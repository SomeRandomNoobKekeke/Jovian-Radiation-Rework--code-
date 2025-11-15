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
  public partial class HullRadProtectionUpgradesModel : RadiationModel
  {
    public class ModelSettings : IConfig
    {
      public float HullUpgradeProtection { get; set; } = 0.1f;
    }

    public override bool Debug { get; set; } = false;

    public override IHullProtectionCalculator HullProtectionCalculator { get; set; }
  }



}