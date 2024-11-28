using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {

    [HarmonyPatch(typeof(CampaignMode))]
    public class CampaignModePatch
    {
      [HarmonyPrefix]
      [HarmonyPatch("HandleSaveAndQuit")]
      public static bool CampaignMode_HandleSaveAndQuit_Prefix(CampaignMode __instance)
      {
        if (!settings.Mod.Progress.ProgressOnSaveLoad) return true;
        if (GameMain.GameSession == null) return true;


        float roundDuration = GameMain.GameSession.RoundDuration;

        float radSteps = roundDuration / (60.0f * settings.Mod.Progress.WorldProgressStepDuration);

        radSteps = Math.Max(0, Math.Min(radSteps, settings.Mod.Progress.WorldProgressMaxStepsPerRound));

        if (radSteps < settings.Mod.Progress.GracePeriod) radSteps = 0;

        radSteps *= settings.Mod.Progress.OutpostTimeMultiplier;

        if (!settings.Mod.Progress.SmoothProgress)
        {
          radSteps = (float)Math.Floor(radSteps);
        }

        Info($"save load Radiation?.OnStep({radSteps})");

        GameMain.GameSession.Map.Radiation?.OnStep(radSteps);

        return true;
      }
    }


  }
}