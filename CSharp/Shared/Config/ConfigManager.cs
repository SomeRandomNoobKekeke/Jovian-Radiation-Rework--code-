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
  public static class ConfigManager
  {
    public static string ConfigID => CurrentConfig is null ?
      null : $"{CurrentConfig.GetType().Namespace}_{CurrentConfig.GetType().Name}";

    private static object _currentConfig;
    public static object CurrentConfig
    {
      get => _currentConfig;
      set
      {
        _currentConfig = value;
        ConfigSaver.Init();
        ConfigNetworking.Init();
        ConfigCommands.UpdateCommand();
      }
    }

    private static string _savePath;
    public static string SavePath
    {
      get => _savePath ?? ConfigSaver.DefaultSavePathFor(CurrentConfig);
      set
      {
        _savePath = value;
        ConfigSaver.Init();
      }
    }

    private static string _commandName;
    public static string CommandName
    {
      get => _commandName;
      set
      {
        _commandName = value;
        ConfigCommands.UpdateCommand();
      }
    }


    private static bool _useAdvancedCommand;
    public static bool UseAdvancedCommand
    {
      get => _useAdvancedCommand;
      set
      {
        _useAdvancedCommand = value;
        ConfigCommands.UpdateCommand();
      }
    }

    public static bool AutoSetup { get; set; } = true;

    public static bool SaveOnQuit { get; set; } = true;
    public static bool SaveEveryRound { get; set; } = true;
    public static bool ShouldSaveInMultiplayer { get; set; } = true;

    public static bool Debug { get; set; } = false;



  }
}