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

using Barotrauma.Extensions;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public Harmony harmony;

    public void Initialize()
    {
      info($"{meta.ModName} - Compiled");

      harmony = new Harmony("radiation.rework");

      patchAll();
      figureOutModVersionAndDirPath();
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
        postfix: new HarmonyMethod(typeof(Mod).GetMethod("init"))
      );


      harmony.Patch(
        original: typeof(Level).GetMethod("DrawBack", AccessTools.all),
        postfix: new HarmonyMethod(typeof(Mod).GetMethod("Level_DrawBack_Postfix"))
      );
    }

    public void OnLoadCompleted() { }
    public void PreInitPatching() { }

    public void Dispose()
    {
      harmony.UnpatchAll(harmony.Id);
      harmony = null;
    }
  }


}