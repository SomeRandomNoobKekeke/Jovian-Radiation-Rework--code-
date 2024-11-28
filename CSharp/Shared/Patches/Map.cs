using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

using Barotrauma.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Voronoi2;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {

    [HarmonyPatch(typeof(Map))]
    public class MapPatch
    {
      [HarmonyPrefix]
      [HarmonyPatch("ProgressWorld", new Type[]{
            typeof(CampaignMode),
            typeof(CampaignMode.TransitionType),
            typeof(float),
          }
        )
      ]
      public static bool Map_ProgressWorld_Replace(CampaignMode campaign, CampaignMode.TransitionType transitionType, float roundDuration, Map __instance)
      {
        Map _ = __instance;

        //one step per 10 minutes of play time
        int steps = (int)Math.Floor(roundDuration / (60.0f * 10.0f));
        if (transitionType == CampaignMode.TransitionType.ProgressToNextLocation ||
            transitionType == CampaignMode.TransitionType.ProgressToNextEmptyLocation)
        {
          //at least one step when progressing to the next location, regardless of how long the round took
          steps = Math.Max(1, steps);
        }
        steps = Math.Min(steps, 5);
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

        _.Radiation?.OnStep(steps);

        return false;
      }
    }


  }
}