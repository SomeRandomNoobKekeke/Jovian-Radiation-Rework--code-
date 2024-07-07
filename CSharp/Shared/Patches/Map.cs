using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

using Barotrauma.Extensions;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static bool Map_ProgressWorld_Replace(CampaignMode campaign, CampaignMode.TransitionType transitionType, float roundDuration, Map __instance)
    {
      if (settings.modSettings.UseVanillaRadiation) return true;

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

      float radSteps = roundDuration / (60.0f * settings.modSettings.Progress.WorldProgressStepDuration);

      radSteps = Math.Max(0, Math.Min(radSteps, settings.modSettings.Progress.WorldProgressMaxStepsPerRound));

      if (transitionType == CampaignMode.TransitionType.ProgressToNextLocation ||
          transitionType == CampaignMode.TransitionType.ProgressToNextEmptyLocation)
      {
        if (radSteps < settings.modSettings.Progress.GracePeriod)
          radSteps = 0;
      }

      if (!settings.modSettings.Progress.SmoothProgress)
      {
        radSteps = (float)Math.Floor(radSteps);
      }

      _.Radiation?.OnStep(radSteps);

      return false;
    }
  }
}