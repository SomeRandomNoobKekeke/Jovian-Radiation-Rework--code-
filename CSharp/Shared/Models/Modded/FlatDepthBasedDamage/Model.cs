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
  public partial class FlatDepthBasedDamageModel : RadiationModel
  {
    public partial class ModelSettings : IConfig
    {
      public float WaterRadiationBlockPerMeter { get; set; } = 0.6f;
    }

    public override IWorldPosRadAmountCalculator WorldPosRadAmountCalculator { get; set; }
  }



}