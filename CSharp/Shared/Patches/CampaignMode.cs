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
      if (!settings.modSettings.Progress.ProgressOnSaveLoad) return true;
      if (GameMain.GameSession == null) return true;


      float roundDuration = GameMain.GameSession.RoundDuration;

      float radSteps = roundDuration / (60.0f * settings.modSettings.Progress.WorldProgressStepDuration);

      radSteps = Math.Max(0, Math.Min(radSteps, settings.modSettings.Progress.WorldProgressMaxStepsPerRound));

      if (radSteps < settings.modSettings.Progress.GracePeriod) radSteps = 0;

      radSteps *= settings.modSettings.Progress.OutpostTimeMultiplier;

      if (!settings.modSettings.Progress.SmoothProgress)
      {
        radSteps = (float)Math.Floor(radSteps);
      }

      info($"save load Radiation?.OnStep({radSteps})");

      GameMain.GameSession.Map.Radiation?.OnStep(radSteps);

      return true;
    }
  }
}