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
  public partial class DepthBasedDamageModel : RadiationModel
  {
    public partial class DepthBasedDamageSettings : IConfig
    {
      public float WaterRadiationBlockPerMeter { get; set; } = 0.6f;
    }

    public DepthBasedDamageSettings Settings => Mod.Config.DepthBasedDamageSettings;

    public override IWorldPosRadAmountCalculator WorldPosRadAmountCalculator { get; set; } = new DepthBasedWorldPosRadAmountCalculator();
  }



}