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
    public enum ConfigSaverResult
    {
      Ok, FileNotFound, ConfigIsNull, NotSupposedTo, Error
    }

    public static bool ShouldSave =>
      GameMain.IsSingleplayer ||
      LuaCsSetup.IsServer ||
      LuaCsSetup.IsClient && ConfigManager.ShouldSaveInMultiplayer;

    public static bool ShouldLoad => true;

    public static string DefaultSavePathFor(object config)
      => Path.Combine(Utils.BarotraumaPath, "ModSettings", "Configs", $"{ConfigManager.ConfigID}.xml");

    public static void Init()
    {
      EnsureDefaultDirectories();
      InstallHooks();

      if (ConfigManager.AutoSetup) LoadSave();
    }

    public static void InstallHooks()
    {
      if (Utils.AlreadyDone()) return;

      GameMain.LuaCs.Hook.Add("stop", $"save {Utils.ModHookId} config on quit", (object[] args) =>
      {
        if (ConfigManager.SaveOnQuit) Save();
        return null;
      });

      GameMain.LuaCs.Hook.Add("roundEnd", $"save {Utils.ModHookId} config on round end", (object[] args) =>
      {
        if (ConfigManager.SaveEveryRound) Save();
        return null;
      });
    }

    public static void EnsureDefaultDirectories()
    {
      if (Utils.AlreadyDone()) return;

      if (!Directory.Exists(Path.Combine(Utils.BarotraumaPath, "ModSettings")))
      {
        Directory.CreateDirectory(Path.Combine(Utils.BarotraumaPath, "ModSettings"));
      }

      if (!Directory.Exists(Path.Combine(Utils.BarotraumaPath, "ModSettings", "Configs")))
      {
        Directory.CreateDirectory(Path.Combine(Utils.BarotraumaPath, "ModSettings", "Configs"));
      }
    }

    public static ConfigSaverResult LoadSave(string path = null)
    {
      ConfigSaverResult result = Load(path);
      Save(path);
      return result;
    }

    public static ConfigSaverResult Save(string path = null)
    {
      if (!ShouldSave)
      {
        ConfigLogging.DebugLog($"-- Can't save config, NotSupposedTo");
        return ConfigSaverResult.NotSupposedTo;
      }
      if (ConfigManager.CurrentConfig is null)
      {
        ConfigLogging.DebugLog($"-- Can't save config, ConfigIsNull");
        return ConfigSaverResult.ConfigIsNull;
      }

      try
      {
        XDocument xdoc = new XDocument();
        xdoc.Add(ConfigSerialization.ToXML(ConfigManager.CurrentConfig));
        xdoc.Save(path ?? ConfigManager.SavePath);
      }
      catch (Exception e)
      {
        ConfigLogging.DebugLog($"-- Can't save config, {e.Message}");
        return ConfigSaverResult.Error;
      }

      return ConfigSaverResult.Ok;
    }


    public static ConfigSaverResult Load(string path = null)
    {
      if (!ShouldLoad)
      {
        ConfigLogging.DebugLog($"-- Can't load config, NotSupposedTo");
        return ConfigSaverResult.NotSupposedTo;
      }

      if (ConfigManager.CurrentConfig is null)
      {
        ConfigLogging.DebugLog($"-- Can't load config, ConfigIsNull");
        return ConfigSaverResult.ConfigIsNull;
      }

      if (!File.Exists(path ?? ConfigManager.SavePath))
      {
        ConfigLogging.DebugLog($"-- Can't load config, FileNotFound [{path ?? ConfigManager.SavePath}]");
        return ConfigSaverResult.FileNotFound;
      }

      try
      {
        XDocument xdoc = XDocument.Load(path ?? ConfigManager.SavePath);
        ConfigSerialization.FromXML(ConfigManager.CurrentConfig, xdoc.Root);
      }
      catch (Exception e)
      {
        ConfigLogging.DebugLog($"-- Can't load config, {e.Message}");
        return ConfigSaverResult.Error;
      }

      return ConfigSaverResult.Ok;
    }



  }
}