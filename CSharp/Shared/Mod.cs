using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace BetterRadiation
{
  public partial class Mod : IAssemblyPlugin
  {
    public Harmony harmony;

    public static float WaterRadiationBlockPerMeter = 0.6f;
    public static float RadiationDamage = 0.037f;
    public static float TooMuchEvenForMonsters = 300;
    public static float HuskRadiationResistance = 0.5f;

    public static bool debug = false;

    public void Initialize()
    {
      harmony = new Harmony("better.radiation");

      patchAll();
      addCommands();

      init();
    }

    public static void init()
    {
      if (GameMain.GameSession?.Map?.Radiation != null)
      {
        GameMain.GameSession.Map.Radiation.Params.RadiationAreaColor = new Color(0, 16, 32, 180);
        GameMain.GameSession.Map.Radiation.Params.RadiationBorderTint = new Color(0, 127, 255, 255);
        //GameMain.GameSession.Map.Radiation.Params.RadiationDamageDelay = 5;
        GameMain.GameSession.Map.Radiation.Params.CriticalRadiationThreshold = 0;
        //GameMain.GameSession.Map.Radiation.Params.MinimumOutpostAmount = 0;

        //GameMain.GameSession.Map.Radiation.Params.BorderAnimationSpeed = 16.66f;
      }
    }

    public static void GameSession_StartRound_Postfix(LevelData? levelData, bool mirrorLevel = false, SubmarineInfo? startOutpost = null, SubmarineInfo? endOutpost = null)
    {
      init();
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
        original: typeof(GameSession).GetMethod("StartRound", AccessTools.all, new Type[]{
          typeof(LevelData),
          typeof(bool),
          typeof(SubmarineInfo),
          typeof(SubmarineInfo),
        }),
        postfix: new HarmonyMethod(typeof(Mod).GetMethod("GameSession_StartRound_Postfix"))
      );
    }

    public static void log(object msg, Color? cl = null)
    {
      if (cl == null) cl = Color.Cyan;
      DebugConsole.NewMessage($"{msg ?? "null"}", cl);
    }
    public static void info(object msg) { if (debug) log(msg); }
    public static void err(object msg) { if (debug) log(msg, Color.Orange); }

    public void OnLoadCompleted() { }
    public void PreInitPatching() { }

    public void Dispose()
    {
      harmony.UnpatchAll(harmony.Id);
      harmony = null;

      removeCommands();
    }
  }


}