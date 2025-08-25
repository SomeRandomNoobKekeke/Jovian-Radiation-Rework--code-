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
    public static bool Debug { get; set; } = false;
    private static string savePath; public static string SavePath
    {
      get => savePath ?? ConfigSaver.DefaultSavePathFor(CurrentConfig);
      set => savePath = value;
    }
    public static bool SaveOnQuit { get; set; } = true;
    public static bool SaveEveryRound { get; set; } = true;
    public static bool ShouldSaveInMultiplayer { get; set; } = true;
    private static object currentConfig; public static object CurrentConfig
    {
      get => currentConfig;
      set { currentConfig = value; Use(value); }
    }

    public static void Load(string path = "")
    {
      ConfigSaver.Load();
      ConfigSaver.Save();
    }

    private static void Use(object config)
    {
      ConfigSaver.Init();
    }


  }
}