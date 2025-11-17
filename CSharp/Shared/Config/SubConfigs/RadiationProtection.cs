using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using BaroJunk;

namespace JovianRadiationRework
{
  public class RadiationProtection : IConfig
  {
    public DepthBasedDamageModel.ModelSettings WaterRadiationBlocking { get; set; }
    public IndoorProtectionModel.ModelSettings HullRadiationProtection { get; set; }
    public HuskRadiationResistanceModel.ModelSettings HuskRadiationProtection { get; set; }
    public HullRadProtectionUpgradesModel.ModelSettings HullRadProtectionUpgrades { get; set; }
  }
}