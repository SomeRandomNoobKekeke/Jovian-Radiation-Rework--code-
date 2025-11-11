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
    public static string ModFolderPath = Path.Combine(ModInfo.ModDir<Mod>(), "Presets");
    public static string DefaultConfigPath = Path.Combine(ModSettingsPath, "Settings.xml");

    public static IEnumerable<string> AvailableConfigs => ModSettingsConfigs;

    // TODO use IOFacade
    public static IEnumerable<string> ModSettingsConfigs
      => Directory.GetFiles(ModSettingsPath, "*.xml")
         .Concat(Directory.GetFiles(ModFolderPath, "*.xml"))
         .Select(path => Path.GetFileNameWithoutExtension(path))
         .Distinct();

    public string GetPathInModSettings(string name) => Path.Combine(ModSettingsPath, $"{name}.xml");
    public string GetPathInModFolder(string name) => Path.Combine(ModInfo.ModDir<Mod>(), "Presets", $"{name}.xml");
    public SimpleResult SaveToModSettings(string name)
    {
      // Actually why not, you can steal others configs
      // if (!this.Self().Manager.AutoSaver.ShouldSave) return SimpleResult.Failure("don't");

      this.Self().Facades.IOFacade.EnsureDirectory(ModSettingsPath);
      return this.Save(GetPathInModSettings(name));
    }

    public SimpleResult LoadPreset(string name)
    {
      if (!this.Self().Manager.AutoSaver.ShouldLoad)
      {
        return SimpleResult.Failure($"You can't load config in {(this.Self().Facades.NetFacade.IsMultiplayer ? "Multiplayer" : "Singleplayer")} with [{Mod.Config.Self().Manager.CurrentStrategy.Name}] config strategy");
      }

      if (this.Self().Facades.IOFacade.FileExists(GetPathInModSettings(name)))
      {
        return this.Load(GetPathInModSettings(name));
      }

      if (this.Self().Facades.IOFacade.FileExists(GetPathInModFolder(name)))
      {
        return this.Load(GetPathInModFolder(name));
      }

      return SimpleResult.Failure("Not found");
    }
  }
}