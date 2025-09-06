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
  public static class TransformLocations
  {

    public static void Vanilla(Radiation _)
    {
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
    }
  }

}