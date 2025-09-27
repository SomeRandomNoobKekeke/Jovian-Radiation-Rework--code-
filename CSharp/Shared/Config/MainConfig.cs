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
    public AmbientLightModel.ModelSettings AmbientLightSettings { get; set; }
    public DepthBasedDamageModel.ModelSettings DepthBasedDamageSettings { get; set; }
    public ProgressiveMonsterSpawningModel.ModelSettings ProgressiveMonsterSpawningSettings { get; set; }

    public ProgressiveCharacterDamagerModel.ModelSettings ProgressiveCharacterDamagerSettings { get; set; }
    public SmoothLocationTransformerModel.ModelSettings SmoothLocationTransformerSettings { get; set; }
    public SmoothRadiationProgressModel.ModelSettings SmoothRadiationProgressSettings { get; set; }


    public List<IConfig> SubSettings = new();

    public IConfig GetSubSettings(Type T)
      => SubSettings.FirstOrDefault(config => config.Type == T);


    public MainConfig()
    {
      this.Restore();
      SubSettings = this.GetSubConfigs().ToList();
    }
  }
}