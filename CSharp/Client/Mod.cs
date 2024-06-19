using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Barotrauma.Networking;
using Barotrauma.Extensions;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static bool HasPermissions => GameMain.Client.IsServerOwner || GameMain.Client.HasPermission(ClientPermissions.All);

    public Harmony harmony;
    public void Initialize()
    {
      harmony = new Harmony("radiation.rework");

      figureOutModVersionAndDirPath();
      createFolders();
      patchAll();
      addCommands();

      if (GameMain.IsSingleplayer)
      {
        settings = Settings.load();
        Settings.save(settings);
        settings.apply();
      }

      if (GameMain.IsMultiplayer)
      {
        GameMain.LuaCs.Networking.Receive("jrr_init", Settings.net_recieve_init);
        GameMain.LuaCs.Networking.Receive("jrr_sync", Settings.net_recieve_sync);

        Settings.askServerForSettings();
      }

      info($"{meta.ModName} | {meta.ModVersion} - Compiled");
    }

    public static void init()
    {
      settings.apply();
    }

    public void patchAll()
    {
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


      harmony.Patch(
        original: typeof(Level).GetMethod("DrawBack", AccessTools.all),
        postfix: new HarmonyMethod(typeof(Mod).GetMethod("Level_DrawBack_Postfix"))
      );

      harmony.Patch(
        original: typeof(LuaGame).GetMethod("IsCustomCommandPermitted"),
        postfix: new HarmonyMethod(typeof(Mod).GetMethod("permitCommands"))
      );
    }

    public void OnLoadCompleted() { }
    public void PreInitPatching() { }

    public void Dispose()
    {
      harmony.UnpatchAll(harmony.Id);
      harmony = null;

      removeCommands();
      serializer = null;
    }
  }


}