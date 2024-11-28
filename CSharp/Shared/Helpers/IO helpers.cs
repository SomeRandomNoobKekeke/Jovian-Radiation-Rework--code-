using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

using System.IO;


namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static string BarotraumaFolder => AppDomain.CurrentDomain.BaseDirectory;
    public static string SettingsFolder => Path.Combine(BarotraumaFolder, $"ModSettings/{ModName}");
    //public static string SettingsFile => Path.Combine(SettingsFolder, $"Settings.xml");
    public static string SettingsFile => Path.Combine(Mod.Instance.ModDir, $"Settings.xml");


    public static void EnsureExists(string path)
    {
      if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    }

    public void FindModFolder()
    {
      bool found = false;

      foreach (ContentPackage p in ContentPackageManager.EnabledPackages.All)
      {
        if (p.Name.Contains(ModName))
        {
          found = true;
          ModDir = Path.GetFullPath(p.Dir);
          ModVersion = p.ModVersion;
          break;
        }
      }

      if (!found) Error($"Couldn't find {ModName} mod folder");
    }
  }
}