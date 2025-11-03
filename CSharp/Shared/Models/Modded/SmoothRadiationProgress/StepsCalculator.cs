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

      public float CalculateSteps(CampaignMode.TransitionType transitionType, float roundDuration)
      {
        float radSteps = roundDuration / (60.0f * Settings.StepDuration);

        radSteps = Math.Max(0, Math.Min(radSteps, Settings.MaxStepsPerRound));

        //TODO is transitionType == LeaveLocation only when leaving outpost?
        if (transitionType == CampaignMode.TransitionType.LeaveLocation)
        {
          radSteps *= Settings.OutpostTimeMultiplier;
        }

        if (transitionType != CampaignMode.TransitionType.ProgressToNextLocation ||
            transitionType != CampaignMode.TransitionType.ProgressToNextEmptyLocation)
        {
          if (roundDuration < Settings.GracePeriod) radSteps = 0;
        }


        return radSteps;
      }
    }
  }
}