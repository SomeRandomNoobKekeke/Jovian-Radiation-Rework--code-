using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static bool CampaignMode_HandleSaveAndQuit_Prefix(CampaignMode __instance)
    {
      float roundDuration = GameMain.GameSession?.RoundDuration ?? 0.0f;

      float radSteps = roundDuration / (60.0f * settings.modSettings.Progress.WorldProgressStepDuration);

      radSteps = Math.Max(0, Math.Min(radSteps, settings.modSettings.Progress.WorldProgressMaxStepsPerRound));

      if (radSteps < settings.modSettings.Progress.GracePeriod) radSteps = 0;

      if (!settings.modSettings.Progress.SmoothProgress)
      {
        radSteps = (float)Math.Floor(radSteps);
      }

      _.Radiation?.OnStep(radSteps);

      return true;
    }
  }
}