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

  /// <summary>
  /// Bruh, i can't dynamically generate it, the only thing i can is to dump model names with ModelManager.DumpModels(); and paste them here
  /// </summary>
  public class EnabledModels : IConfig
  {
    // public bool VanillaRadiationModel { get; set; } = true;
    public bool AdditionalMetadataSetterModel { get; set; } = true;
    public bool AdvanceOnSaveAndQuitModel { get; set; } = true;
    public bool AmbientLightModel { get; set; } = true;
    public bool CustomCharacterDamager { get; set; } = true;
    public bool CustomRadiationUpdaterModel { get; set; } = true;
    public bool DamageToElectronicsModel { get; set; } = true;
    public bool FlatDepthBasedDamageModel { get; set; } = true;
    public bool ProgressiveMonsterSpawningModel { get; set; } = true;
    public bool SmoothLocationTransformerModel { get; set; } = true;
    public bool SmoothRadiationProgressModel { get; set; } = true;
    public bool HullBlocksRadiationModel { get; set; } = true;
    public bool HuskRadiationResistanceModel { get; set; } = true;
    public bool CustomTooltipModel { get; set; } = true;

    public EnabledModels()
    {
      this.OnPropChanged((key, value) =>
      {
        Mod.ModelManager.SetModelState(key, (bool)value);
      });

      this.OnUpdated(() => Mod.ModelManager.SyncModelStates(this));
    }
  }
}