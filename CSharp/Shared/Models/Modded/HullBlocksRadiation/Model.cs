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
  public partial class HullBlocksRadiationModel : RadiationModel
  {
    public class ModelSettings : IConfig
    {
      public float FractionOfRadiationBlockedInSub { get; set; } = 0.0f;
    }

    public override IEntityRadAmountCalculator EntityRadAmountCalculator { get; set; }
  }



}