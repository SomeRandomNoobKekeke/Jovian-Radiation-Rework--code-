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
        float mins = roundDuration / 60.0f;

        mins = Math.Max(0, Math.Min(mins, Settings.MaxTimeOnRound));
        if (mins < Settings.MinTimeOnRound) mins = 0;

        //TODO is transitionType == LeaveLocation only when leaving outpost?
        if (transitionType == CampaignMode.TransitionType.LeaveLocation)
        {
          mins *= Settings.OutpostTimeMultiplier;
        }

        // Why grace period should be applied only on outposts? i don't remember
        // if (transitionType != CampaignMode.TransitionType.ProgressToNextLocation ||
        //     transitionType != CampaignMode.TransitionType.ProgressToNextEmptyLocation)
        // {
        //   if (mins < Settings.MinTimeOnRound) mins = 0;
        // }


        return mins;
      }
    }
  }
}