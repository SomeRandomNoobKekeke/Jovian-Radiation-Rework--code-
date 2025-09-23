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

  public static class MapPatch
  {
    public static void PatchSharedMap(Harmony harmony)
    {
      harmony.Patch(
        original: typeof(Map).GetMethod("ProgressWorld", AccessTools.all, new Type[]{
          typeof(CampaignMode),
          typeof(CampaignMode.TransitionType),
          typeof(float),
        }),
        prefix: new HarmonyMethod(typeof(MapPatch).GetMethod("Map_ProgressWorld_Replace"))
      );
    }


    // https://github.com/evilfactory/LuaCsForBarotrauma/blob/master/Barotrauma/BarotraumaShared/SharedSource/Map/Map/Map.cs#L1222
    public static bool Map_ProgressWorld_Replace(CampaignMode campaign, CampaignMode.TransitionType transitionType, float roundDuration, Map __instance)
    {

      Map _ = __instance;

      float steps = Mod.LogicContainer.WorldProgressStepsCalculator.CalculateSteps(transitionType, roundDuration);

      for (int i = 0; i < steps; i++)
      {
        _.ProgressWorld(campaign);
      }

      // always update specials every step
      for (int i = 0; i < Math.Max(1, steps); i++)
      {
        foreach (Location location in _.Locations)
        {
          if (!location.Discovered) { continue; }
          location.UpdateSpecials();
        }
      }


      steps = Mod.LogicContainer.RadiationStepsCalculator.CalculateSteps(transitionType, roundDuration);

      _.Radiation?.OnStep(steps);


      return false;
    }
  }

}