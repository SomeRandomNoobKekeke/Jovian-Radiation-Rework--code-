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
  public partial class MainConfig : IConfig
  {
    public EnabledModels EnabledModels { get; set; }


    public Visuals Visuals { get; set; }
    public RadiationEffects RadiationEffects { get; set; }
    public RadiationProgress RadiationProgress { get; set; }
    public RadiationProtection RadiationProtection { get; set; }

    public VanillaSettings Vanilla { get; set; }


    private IEnumerable<IConfig> SubConfigs
      => this.GetAllEntriesRec()
      .Where(entry => entry.IsConfig)
      .Select(entry => entry.Value as IConfig);

    public List<IConfig> SubSettings = new();

    public IConfig GetSubSettings(Type T)
    {
      if (T is null) return null;
      IConfig settings = SubSettings.FirstOrDefault(config =>
      {
        return config.GetCore().RawTarget.GetType() == T;
      });
      if (settings is null) throw new Exception($"[{T.DeclaringType.Name}.{T.Name}] not found in MainConfig");
      return settings;
    }


    public MainConfig()
    {
      this.Restore();
      SubSettings = SubConfigs.ToList();
    }
  }
}