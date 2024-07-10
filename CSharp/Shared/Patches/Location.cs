using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static bool Location_IsCriticallyRadiated_Replace(Location __instance, ref bool __result)
    {
      if (GameMain.GameSession?.Map?.Radiation != null)
      {
        __result = GameMain.GameSession.Map.Radiation.Params.CriticalRadiationThreshold >= 0 && __instance.TurnsInRadiation > GameMain.GameSession.Map.Radiation.Params.CriticalRadiationThreshold;

        __result = __result || settings.modSettings.Progress.CriticalOutpostRadiationAmount >= 0 && GameMain.GameSession.Map.Radiation.Amount - __instance.MapPosition.X > settings.modSettings.Progress.CriticalOutpostRadiationAmount;

        return false;
      }

      __result = false;
      return false;
    }
  }
}