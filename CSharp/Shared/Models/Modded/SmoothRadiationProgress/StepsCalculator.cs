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
  public partial class SmoothRadiationProgressModel
  {
    public class SmoothRadStepsCalculator : IStepsCalculator
    {
      public ModelSettings Settings { get; set; }
      public SmoothRadiationProgressModel Model { get; set; }

      /// <summary>
      /// Step = 1 min
      /// </summary>
      public float CalculateSteps(CampaignMode.TransitionType transitionType, float roundDuration)
      {
        float steps = roundDuration / 60.0f;

        steps = Math.Max(0, Math.Min(steps, Settings.MaxTimeOnRound));
        if (steps < Settings.MinTimeOnRound) steps = 0;

        //TODO is transitionType == LeaveLocation only when leaving outpost?
        if (transitionType == CampaignMode.TransitionType.LeaveLocation)
        {
          steps *= Settings.OutpostTimeMultiplier;
        }

        // Why grace period should be applied only on outposts? i don't remember
        // if (transitionType != CampaignMode.TransitionType.ProgressToNextLocation ||
        //     transitionType != CampaignMode.TransitionType.ProgressToNextEmptyLocation)
        // {
        //   if (steps < Settings.MinTimeOnRound) steps = 0;
        // }

        Model.DebugLog($"roundDuration:[{roundDuration}] steps:[{steps}]");


        return steps;
      }
    }
  }
}