using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

using Barotrauma.Extensions;
using System.IO;
using Barotrauma.Networking;
using System.Text.Json;
using System.Text.Json.Serialization;

[assembly: IgnoresAccessChecksTo("Barotrauma")]
[assembly: IgnoresAccessChecksTo("DedicatedServer")]
[assembly: IgnoresAccessChecksTo("BarotraumaCore")]
namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static ModMetadata meta = new ModMetadata();
    public static Settings settings = new Settings();
    public static bool debug = false;

    public Harmony harmony;

    public void Initialize()
    {
      harmony = new Harmony("radiation.rework");

      figureOutModVersionAndDirPath();
      createFolders();
      PatchOnBothSides();
#if CLIENT
      PatchOnClient();
      InitializeClient();
#elif SERVER
      PatchOnServer();
      InitializeServer();
#endif

      info($"{meta.ModName} | {meta.ModVersion} - Compiled");
    }

    public static void init()
    {
      settings.apply();
    }

    public void PatchOnBothSides()
    {
      harmony.Patch(
        original: typeof(ColorExtensions).GetMethod("Multiply", AccessTools.all, new Type[]{
        typeof(Color),
        typeof(float),
        typeof(bool),
      }),
        prefix: new HarmonyMethod(typeof(Mod).GetMethod("ColorExtensions_Multiply_Prefix"))
      );

      harmony.Patch(
        original: typeof(MonsterEvent).GetMethod("InitEventSpecific", AccessTools.all),
        prefix: new HarmonyMethod(typeof(Mod).GetMethod("MonsterEvent_InitEventSpecific_Replace"))
      );

      harmony.Patch(
        original: typeof(Radiation).GetMethod("UpdateRadiation", AccessTools.all),
        prefix: new HarmonyMethod(typeof(Mod).GetMethod("Radiation_UpdateRadiation_Replace"))
      );

      harmony.Patch(
        original: typeof(Radiation).GetMethod("OnStep", AccessTools.all),
        prefix: new HarmonyMethod(typeof(Mod).GetMethod("Radiation_OnStep_Replace"))
      );

      // not yet, might be hard to sync
      // harmony.Patch(
      //   original: typeof(Radiation).GetConstructors()[0],
      //   postfix: new HarmonyMethod(typeof(Mod).GetMethod("Radiation_Constructor_Postfix"))
      // );

      harmony.Patch(
        original: typeof(Map).GetMethod("ProgressWorld", AccessTools.all, new Type[]{
          typeof(CampaignMode),
          typeof(CampaignMode.TransitionType),
          typeof(float),
        }),
        prefix: new HarmonyMethod(typeof(Mod).GetMethod("Map_ProgressWorld_Replace"))
      );

      harmony.Patch(
        original: typeof(GameSession).GetMethod("StartRound", AccessTools.all, new Type[]{
          typeof(LevelData),
          typeof(bool),
          typeof(SubmarineInfo),
          typeof(SubmarineInfo),
        }),
        postfix: new HarmonyMethod(typeof(Mod).GetMethod("init"))
      );
    }
  }
}