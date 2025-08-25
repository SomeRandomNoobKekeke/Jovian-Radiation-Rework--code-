using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace JovianRadiationRework
{
  public static class ConfigSaver
  {
    public static object CurrentConfig { get; set; }

    public static bool Verbose = true;
    public static bool SaveOnQuit { get; set; } = true;
    public static bool SaveEveryRound { get; set; } = true;
    public static string SavePath { get; set; }
    public static bool ShouldSaveInMultiplayer { get; set; } = true;
    public static bool ShouldSave =>
      GameMain.IsSingleplayer ||
      LuaCsSetup.IsServer ||
      LuaCsSetup.IsClient && ShouldSaveInMultiplayer;

    public static string DefaultSavePathFor(object config)
      => Path.Combine(BarotraumaPath, "ModSettings", "Configs", $"{config.GetType().Namespace}_{config.GetType().Name}.xml");

    public static void Use(object config, string path = null)
    {
      ArgumentNullException.ThrowIfNull(config);
      EnsureDefaultDirectories();

      CurrentConfig = config;
      string id = $"{CurrentConfig.GetType().Namespace}_{CurrentConfig.GetType().Name}";

      InstallHooks(id);

      SavePath = path ?? DefaultSavePathFor(config);

      TryLoad(); Save();
    }

    private static bool HooksInstalled;
    public static void InstallHooks(string id)
    {
      if (HooksInstalled) return;
      HooksInstalled = true;

      GameMain.LuaCs.Hook.Add("stop", $"save {id} on quit", (object[] args) =>
      {
        if (SaveOnQuit) Save();
        return null;
      });

      GameMain.LuaCs.Hook.Add("roundEnd", $"save {id} on round end", (object[] args) =>
      {
        if (SaveEveryRound) Save();
        return null;
      });
    }

    public static void EnsureDefaultDirectories()
    {
      if (!Directory.Exists(Path.Combine(BarotraumaPath, "ModSettings")))
      {
        Directory.CreateDirectory(Path.Combine(BarotraumaPath, "ModSettings"));
      }

      if (!Directory.Exists(Path.Combine(BarotraumaPath, "ModSettings", "Configs")))
      {
        Directory.CreateDirectory(Path.Combine(BarotraumaPath, "ModSettings", "Configs"));
      }
    }

    public static bool SafeSave() => _Save(true);
    public static bool TrySave() => _Save(false);
    public static bool Save() => _Save(Verbose);
    private static bool _Save(bool verbose)
    {
      if (!ShouldSave) return false;

      if (string.IsNullOrEmpty(SavePath))
      {
        if (verbose) Mod.Warning($"-- Can't save config, SavePath is empty");
        return false;
      }
      if (CurrentConfig is null)
      {
        if (verbose) Mod.Warning($"-- Can't save config, CurrentConfig is null");
        return false;
      }

      try
      {
        XDocument xdoc = new XDocument();
        xdoc.Add(ConfigSerialization.ToXML(CurrentConfig));
        xdoc.Save(SavePath);
      }
      catch (Exception e)
      {
        if (verbose) Mod.Warning($"-- Can't save config, {e.Message}");
        return false;
      }

      return true;
    }

    public static bool SafeLoad() => _Load(true);
    public static bool TryLoad() => _Load(false);
    public static bool Load() => _Load(Verbose);

    private static bool _Load(bool verbose)
    {
      if (string.IsNullOrEmpty(SavePath))
      {
        if (verbose) Mod.Warning($"-- Can't load config, SavePath is empty");
        return false;
      }
      if (CurrentConfig is null)
      {
        if (verbose) Mod.Warning($"-- Can't load config, CurrentConfig is null");
        return false;
      }
      if (!File.Exists(SavePath))
      {
        if (verbose) Mod.Warning($"-- Can't load config, file doesn't exist");
        return false;
      }

      try
      {
        XDocument xdoc = XDocument.Load(SavePath);
        ConfigSerialization.FromXML(CurrentConfig, xdoc.Root);
      }
      catch (Exception e)
      {
        if (verbose) Mod.Warning($"-- Can't load config, {e.Message}");
        return false;
      }

      return true;
    }



  }
}