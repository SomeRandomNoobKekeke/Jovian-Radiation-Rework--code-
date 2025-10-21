using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using BaroJunk;
using BaroJunk_Config;


namespace JovianRadiationRework
{
  public class MainConfig : IConfig
  {
    public EnabledModels EnabledModels { get; set; }
    public AmbientLightModel.ModelSettings AmbientLightSettings { get; set; }
    public FlatDepthBasedDamageModel.ModelSettings FlatDepthBasedDamageSettings { get; set; }
    public ProgressiveMonsterSpawningModel.ModelSettings ProgressiveMonsterSpawningSettings { get; set; }

    public SmoothCharacterDamager.ModelSettings SmoothCharacterDamagerSettings { get; set; }
    public SmoothLocationTransformerModel.ModelSettings SmoothLocationTransformerSettings { get; set; }
    public SmoothRadiationProgressModel.ModelSettings SmoothRadiationProgressSettings { get; set; }
    public DamageToElectronicsModel.ModelSettings DamageToElectronicsSettings { get; set; }
    public AdvanceOnSaveAndQuitModel.ModelSettings AdvanceOnSaveAndQuitSettings { get; set; }
    public HullBlocksRadiationModel.ModelSettings HullBlocksRadiationSettings { get; set; }


    public IEnumerable<IConfig> SubConfigs
      => this.GetAllEntriesRec()
      .Where(entry => entry.IsConfig)
      .Select(entry => entry.Value as IConfig);

    public List<IConfig> SubSettings = new();

    public IConfig GetSubSettings(Type T)
    {
      if (T is null) return null;
      IConfig settings = SubSettings.FirstOrDefault(config => config.GetCore().RawTarget.GetType() == T);
      if (settings is null) throw new Exception($"[{T.DeclaringType.Name}.{T.Name}] not found in MainConfig");
      return settings;
    }


    public MainConfig()
    {
      this.Restore();
      SubSettings = SubConfigs.ToList();

      //TODO make IConfig deeply reactive
      #region Cringe
      this.OnPropChanged((path, state) =>
      {
        if (!path.StartsWith("EnabledModels")) return;
        path = path.Remove(0, "EnabledModels.".Length);
        EnabledModels.RaiseModelToggled(path, (bool)state);
      });
      #endregion
    }
  }
}