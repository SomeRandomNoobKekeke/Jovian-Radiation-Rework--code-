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
  public class MainConfig : IConfig
  {
    public AmbientLightModel.AmbientLightSettings AmbientLightSettings { get; set; }
    public DepthBasedDamageModel.DepthBasedDamageSettings DepthBasedDamageSettings { get; set; }
    public ProgressiveMonsterSpawningModel.ProgressiveMonsterSpawningSettings ProgressiveMonsterSpawningSettings { get; set; }


    public MainConfig()
    {
      this.Restore();
    }
  }
}