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
    public partial class SmoothRadiationProgressSettings : IConfig
    {
      public float TargetSpeedPercentageAtTheEndOfTheMap { get; set; } = 0.5f;
      public float RadiationSpeed { get; set; } = 20.0f;
    }

    public SmoothRadiationProgressSettings Settings => Mod.Config.SmoothRadiationProgressSettings;

    public override IRadiationMover RadiationMover { get; set; } = new SmoothRadiationMover();
  }



}