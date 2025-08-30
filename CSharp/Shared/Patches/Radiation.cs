using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

namespace JovianRadiationRework
{
  public class RadiationPatches
  {
    public static void PatchSharedRadiation(Harmony harmony)
    {
      harmony.Patch(
        original: typeof(Radiation).GetMethod("OnStep", AccessTools.all),
        prefix: new HarmonyMethod(typeof(RadiationPatches).GetMethod("Radiation_OnStep_Replace"))
      );

      harmony.Patch(
        original: typeof(Radiation).GetMethod("UpdateRadiation", AccessTools.all),
        prefix: new HarmonyMethod(typeof(RadiationPatches).GetMethod("Radiation_UpdateRadiation_Replace"))
      );
    }

    public static bool Radiation_OnStep_Replace(Radiation __instance, float steps = 1)
    {
      Radiation _ = __instance;

      if (!_.Enabled) { return false; }
      if (steps <= 0) { return false; }

      float increaseAmount = _.Params.RadiationStep * steps;

      if (_.Params.MaxRadiation > 0 && _.Params.MaxRadiation < _.Amount + increaseAmount)
      {
        increaseAmount = _.Params.MaxRadiation - _.Amount;
      }

      _.IncreaseRadiation(increaseAmount);

      int amountOfOutposts = _.Map.Locations.Count(location => location.Type.HasOutpost && !location.IsCriticallyRadiated());

      foreach (Location location in _.Map.Locations.Where(l => _.DepthInRadiation(l) > 0))
      {
        if (location.IsGateBetweenBiomes)
        {
          location.Connections.ForEach(c => c.Locked = false);
          continue;
        }

        if (amountOfOutposts <= _.Params.MinimumOutpostAmount) { break; }

        if (_.Map.CurrentLocation is { } currLocation)
        {
          // Don't advance on nearby locations to avoid buggy behavior
          if (currLocation == location || currLocation.Connections.Any(lc => lc.OtherLocation(currLocation) == location)) { continue; }
        }

        bool wasCritical = location.IsCriticallyRadiated();

        location.TurnsInRadiation++;

        if (location.Type.HasOutpost && !wasCritical && location.IsCriticallyRadiated())
        {
          location.ClearMissions();
          amountOfOutposts--;
        }
      }

      return false;
    }

    public static bool Radiation_UpdateRadiation_Replace(Radiation __instance, float deltaTime)
    {
      Radiation _ = __instance;

      if (!(GameMain.GameSession?.IsCurrentLocationRadiated() ?? false)) { return false; }

      if (GameMain.NetworkMember is { IsClient: true }) { return false; }

      if (_.radiationTimer > 0)
      {
        _.radiationTimer -= deltaTime;
        return false;
      }

      _.radiationTimer = _.Params.RadiationDamageDelay;

      foreach (Character character in Character.CharacterList)
      {
        DamageCharacters.Vanilla(_, character);
      }

      return false;
    }
  }

}