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
          __result = __instance.TurnsInRadiation > GameMain.GameSession.Map.Radiation.Params.CriticalRadiationThreshold;
          return false;
        }

        __result = false;
        return false;
      }
    }


  }
}