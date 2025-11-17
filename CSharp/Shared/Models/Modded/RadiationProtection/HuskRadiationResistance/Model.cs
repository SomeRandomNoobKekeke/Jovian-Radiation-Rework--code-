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
  public partial class HuskRadiationResistanceModel : RadiationModel
  {
    public class ModelSettings : IConfig
    {
      public float HuskRadiationResistanceMult;
      private float huskRadiationResistance = 0.5f;
      public float HuskRadiationResistance
      {
        get => huskRadiationResistance;
        set
        {
          huskRadiationResistance = value;
          HuskRadiationResistanceMult = Math.Clamp(1 - value, 0, 1);
        }
      }

      public float HuskInfectionThreshold { get; set; } = 75.0f;
    }
    public override bool Debug { get; set; } = false;

    public override IHuskResistanceCalculator HuskResistanceCalculator { get; set; }
  }



}