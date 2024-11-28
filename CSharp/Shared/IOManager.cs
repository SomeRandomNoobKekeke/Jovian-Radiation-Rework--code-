using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace JovianRadiationRework
{
  public class IOManager
  {
    public static string BarotraumaFolder => AppDomain.CurrentDomain.BaseDirectory;
    public static string SettingsFolder => Path.Combine(BarotraumaFolder, $"ModSettings/{Mod.ModName}");
    public static string SavedPresets => Path.Combine(SettingsFolder, "Presets");
    public static string ModPresets => Path.Combine(Mod.Instance.ModDir, "Presets");
    public static string SettingsFile => Path.Combine(SettingsFolder, $"Settings.xml");
    //public static string SettingsFile => Path.Combine(Mod.Instance.ModDir, $"Settings.xml");
    public static string DefaultPreset => Path.Combine(ModPresets, "Default.xml");
    public static string VanillaPreset => Path.Combine(ModPresets, "Vanilla.xml");

    public static void EnsureExists(string path)
    {
      if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    }

    public static void EnsureStuff()
    {
      EnsureExists(SettingsFolder);
      EnsureExists(SavedPresets);
    }

    public static bool SettingsExist => File.Exists(SettingsFile);

    public static Dictionary<string, string> AllPresets()
    {
      Dictionary<string, string> allPresets = new Dictionary<string, string>();

      foreach (string p in Directory.GetFiles(ModPresets))
      {
        if (Path.GetExtension(p) != ".xml") continue;
        allPresets[Path.GetFileNameWithoutExtension(p)] = p;
      }

      foreach (string p in Directory.GetFiles(SavedPresets))
      {
        if (Path.GetExtension(p) != ".xml") continue;
        allPresets[Path.GetFileNameWithoutExtension(p)] = p;
      }

      return allPresets;
    }


  }
}