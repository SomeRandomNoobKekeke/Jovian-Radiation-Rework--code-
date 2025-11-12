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

  public class EnabledModels : IConfig
  {
    public bool AdvanceOnSaveAndQuitModel { get; set; } = true;
    public bool DamageToElectronicsModel { get; set; } = true;
    public bool FlatDepthBasedDamageModel { get; set; } = true;
    public bool HullBlocksRadiationModel { get; set; } = true;
    public bool ProgressiveMonsterSpawningModel { get; set; } = true;
    public bool SmoothCharacterDamager { get; set; } = true;
    public bool SmoothLocationTransformerModel { get; set; } = true;
    public bool SmoothRadiationProgressModel { get; set; } = true;
    public bool AmbientLightModel { get; set; } = true;

    public EnabledModels()
    {
      this.OnPropChanged((key, value) =>
      {
        Mod.ModelManager.SetModelState(key, (bool)value);
      });
    }
  }
}