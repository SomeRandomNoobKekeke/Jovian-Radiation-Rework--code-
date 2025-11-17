using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using BaroJunk;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public partial void PatchProjSpecific()
    {
      LevelPatch.PatchClientLevel(Harmony);
      LuaGamePatch.PatchClientLuaGame(Harmony);
      RadiationPatch.PatchClientRadiation(Harmony);
      UpgradeStorePatch.PatchClientUpgradeStore(Harmony);
      // LevelRendererPatch.PatchClientLevelRenderer(Harmony);
    }

    public partial void SetupHooksProjSpecific()
    {
      GameMain.LuaCs.Hook.Add("GeigerCounterTurnedOff", "JRR", GeigerCounterHooks.OnTurnedOff);
      GameMain.LuaCs.Hook.Add("MeasureRadiation", "JRR", GeigerCounterHooks.MeasureRadiation);
    }
  }
}