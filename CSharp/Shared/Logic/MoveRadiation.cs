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
  public static class MoveRadiation
  {

    public static void Vanilla(Radiation _, CampaignMode.TransitionType transitionType, float roundDuration)
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

      if (steps <= 0) { return; }

      float increaseAmount = _.Params.RadiationStep * steps;

      if (_.Params.MaxRadiation > 0 && _.Params.MaxRadiation < _.Amount + increaseAmount)
      {
        increaseAmount = _.Params.MaxRadiation - _.Amount;
      }

      _.IncreaseRadiation(increaseAmount);
    }
  }

}