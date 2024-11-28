using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;


namespace JovianRadiationRework
{
  public class ProgressSettings
  {
    public float WorldProgressStepDuration { get; set; } = 1.0f;
    public float WorldProgressMaxStepsPerRound { get; set; } = 60.0f;
    public bool KeepSurroundingOutpostsAlive { get; set; } = true;
    public bool SmoothProgress { get; set; } = true;
    public bool ProgressOnSaveLoad { get; set; } = true;
    public float GracePeriod { get; set; } = 1.0f;
    public float OutpostTimeMultiplier { get; set; } = 0.5f;
    public float TargetSpeedPercentageAtTheEndOfTheMap { get; set; } = 1.0f;
    public float CriticalOutpostRadiationAmount { get; set; } = -1.0f;

    public ProgressSettings() { }
  }
}