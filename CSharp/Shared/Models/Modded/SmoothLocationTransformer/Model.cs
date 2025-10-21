using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using BaroJunk_Config;


namespace JovianRadiationRework
{
  public partial class SmoothLocationTransformerModel : RadiationModel
  {
    public partial class ModelSettings : IConfig
    {
      public bool KeepSurroundingOutpostsAlive { get; set; } = true;
      public float CriticalOutpostRadiationAmount { get; set; } = 0.0f;
      public int MinimumOutpostAmount { get; set; } = 3;

    }

    public override ILocationTransformer LocationTransformer { get; set; }
    public override ILocationIsCriticallyRadiated LocationIsCriticallyRadiated { get; set; }
  }



}