using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {

    [HarmonyPatch(typeof(Location))]
    public class LocationPatch
    {
      [HarmonyPrefix]
      [HarmonyPatch("IsCriticallyRadiated")]
      public static bool Location_IsCriticallyRadiated_Replace(ref bool __result, Location __instance)
      {
        if (GameMain.GameSession?.Map?.Radiation != null)
        {
          __result = GameMain.GameSession.Map.Radiation.Params.CriticalRadiationThreshold >= 0 && __instance.TurnsInRadiation > GameMain.GameSession.Map.Radiation.Params.CriticalRadiationThreshold;

          __result = __result || settings.Mod.Progress.CriticalOutpostRadiationAmount >= 0 && GameMain.GameSession.Map.Radiation.Amount - __instance.MapPosition.X > settings.Mod.Progress.CriticalOutpostRadiationAmount;

          return false;
        }

        __result = false;
        return false;
      }
    }


  }
}