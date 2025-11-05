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
using System.IO;

namespace JovianRadiationRework
{
  // This is confusing
  // I already have save and load methods in IConfig,
  // but i want to save to a specific folder, and i want to localize this logic somewhere
  // Also if i call them save, load it will hide IConfig implementations and break everything
  public partial class MainConfig : IConfig
  {
    public static string ModSettingsPath = "ModSettings/Jovian Radiation Rework";

    public SimpleResult SaveToModSettings(string name)
    {
      this.Self().Facades.IOFacade.EnsureDirectory(ModSettingsPath);
      return this.Save(Path.Combine(ModSettingsPath, $"{name}.xml"));
    }

    public SimpleResult LoadFromModSettings(string name)
    {
      if (!this.Self().Facades.IOFacade.FileExists(Path.Combine(ModSettingsPath, $"{name}.xml")))
      {
        return SimpleResult.Failure($"[{Path.Combine(ModSettingsPath, $"{name}.xml")}] file doesn't exist");
      }

      return this.Load(Path.Combine(ModSettingsPath, $"{name}.xml"));
    }
  }
}