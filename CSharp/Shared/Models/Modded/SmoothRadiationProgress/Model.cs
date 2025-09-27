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
  public partial class SmoothRadiationProgressModel : RadiationModel
  {
    public partial class ModelSettings : IConfig
    {
      public float TargetSpeedPercentageAtTheEndOfTheMap { get; set; } = 0.5f;
      public float RadiationSpeed { get; set; } = 100.0f;
      public float StepDuration { get; set; } = 1.0f;
      public float MaxStepsPerRound { get; set; } = 10.0f;
      public float GracePeriod { get; set; } = 10.0f;

      public float OutpostTimeMultiplier { get; set; } = 0.25f;


    }

    public override IRadiationMover RadiationMover { get; set; }
    public override IStepsCalculator RadiationStepsCalculator { get; set; }
  }



}