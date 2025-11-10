using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using BaroJunk;

using System.IO;

namespace JovianRadiationRework
{
  // This is confusing
  // I already have save and load methods in IConfig,
  // but i want to save to a specific folder, and i want to localize this logic somewhere
  // Also if i name them save, load it will hide IConfig implementations and break everything
  public partial class MainConfig : IConfig
  {
    public static string ModSettingsPath = Path.Combine("ModSettings", "Jovian Radiation Rework");
    public static IEnumerable<string> AvailableConfigs => ModSettingsConfigs;

    // TODO use IOFacade
    public static IEnumerable<string> ModSettingsConfigs
      => Directory.GetFiles(ModSettingsPath, "*.xml")
         .Select(path => Path.GetFileNameWithoutExtension(path));

    public string GetPathInModSettings(string name) => Path.Combine(ModSettingsPath, $"{name}.xml");
    public SimpleResult SaveToModSettings(string name)
    {
      // Actually why not, you can steal others configs
      // if (!this.Self().Manager.AutoSaver.ShouldSave) return SimpleResult.Failure("don't");

      this.Self().Facades.IOFacade.EnsureDirectory(ModSettingsPath);
      return this.Save(GetPathInModSettings(name));
    }

    public SimpleResult LoadFromModSettings(string name)
    {
      if (!this.Self().Manager.AutoSaver.ShouldLoad)
      {
        return SimpleResult.Failure($"You can't load config in {(this.Self().Facades.NetFacade.IsMultiplayer ? "Multiplayer" : "Singleplayer")} with [{Mod.Config.Self().Manager.CurrentStrategy.Name}] config strategy");
      }

      if (!this.Self().Facades.IOFacade.FileExists(GetPathInModSettings(name)))
      {
        return SimpleResult.Failure($"[{GetPathInModSettings(name)}] file doesn't exist");
      }

      return this.Load(GetPathInModSettings(name));
    }
  }
}