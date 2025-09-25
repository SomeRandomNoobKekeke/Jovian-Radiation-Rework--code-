using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

using Barotrauma.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Voronoi2;


namespace JovianRadiationRework
{
  public partial class SmoothLocationTransformerModel
  {
    public class SmoothLocationTransformer : ILocationTransformer
    {
      public SmoothLocationTransformerModel Model { get; set; }

      public void TransformLocations(Radiation _)
      {
        int amountOfOutposts = _.Map.Locations.Count(location => location.Type.HasOutpost && !location.IsCriticallyRadiated());

        foreach (Location location in _.Map.Locations.Where(l => _.DepthInRadiation(l) > 0))
        {
          if (location.IsGateBetweenBiomes)
          {
            location.Connections.ForEach(c => c.Locked = false);
            // continue;
          }

          if (amountOfOutposts <= _.Params.MinimumOutpostAmount) { break; }

          if (Model.Settings.KeepSurroundingOutpostsAlive && _.Map.CurrentLocation is { } currLocation)
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




}