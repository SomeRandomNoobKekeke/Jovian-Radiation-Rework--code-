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
  public partial class SmoothLocationTransformerModel : RadiationModel
  {
    public partial class SmoothLocationTransformerSettings : IConfig
    {
      public bool KeepSurroundingOutpostsAlive { get; set; } = true;
      public float CriticalOutpostRadiationAmount { get; set; } = 0.0f;

    }

    public SmoothLocationTransformerSettings Settings => Mod.Config.SmoothLocationTransformerSettings;

    public override ILocationTransformer LocationTransformer { get; set; } = new SmoothLocationTransformer();
    public override ILocationIsCriticallyRadiated LocationIsCriticallyRadiated { get; set; } = new SmoothLocationIsCriticallyRadiated();
  }



}