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
    public FlatDepthBasedDamageModel.ModelSettings WaterRadiationBlocking { get; set; }
    public HullBlocksRadiationModel.ModelSettings HullRadiationProtection { get; set; }
    public HuskRadiationResistanceModel.ModelSettings HuskRadiationProtection { get; set; }
  }
}