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
  public partial class VanillaRadiationModel
  {
    public class VanillaStepsCalculator : IStepsCalculator
    {
      public float CalculateSteps(CampaignMode.TransitionType transitionType, float roundDuration)
      {
        //one step per 10 minutes of play time
        int steps = (int)Math.Floor(roundDuration / (60.0f * 10.0f));
        if (transitionType == CampaignMode.TransitionType.ProgressToNextLocation ||
            transitionType == CampaignMode.TransitionType.ProgressToNextEmptyLocation)
        {
          //at least one step when progressing to the next location, regardless of how long the round took
          steps = Math.Max(1, steps);
        }
        steps = Math.Min(steps, 5);

        return steps;
      }
    }
  }
}