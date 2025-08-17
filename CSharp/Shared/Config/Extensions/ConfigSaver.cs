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
    public static string BarotraumaPath => Path.GetFullPath("./");

    public static ContentPackage ModPackage<PluginType>() where PluginType : IAssemblyPlugin
    {
      GameMain.LuaCs.PluginPackageManager.TryGetPackageForPlugin<PluginType>(out ContentPackage package);
      return package;
    }
    public static string ModPath<PluginType>() where PluginType : IAssemblyPlugin => ModPackage<PluginType>().Dir;
    public static string ModVersion<PluginType>() where PluginType : IAssemblyPlugin => ModPackage<PluginType>().ModVersion;
    public static string ModName<PluginType>() where PluginType : IAssemblyPlugin => ModPackage<PluginType>().Name;

    public static bool SaveEveryRound { get; set; } = true;
    public static string SavePath { get; set; }

    public static bool Save()
    {
      // if (CurrentConfig is null) return false;
      // if (SavePath is null) return false;
      // if (!File.Exists(path)) return false;

      // XDocument xdoc = XDocument.Load(path);
      // ConfigSerialization.FromXML(CurrentConfig, xdoc.Root);

      return true;
    }

    public static bool Load()
    {
      if (CurrentConfig is null) return false;
      if (string.IsNullOrEmpty(SavePath)) return false;
      if (!File.Exists(SavePath)) return false;

      XDocument xdoc = XDocument.Load(SavePath);
      ConfigSerialization.FromXML(CurrentConfig, xdoc.Root);

      return true;
    }

    // public void Save(string path)
    // {
    //   XDocument xdoc = new XDocument();
    //   xdoc.Add(this.ToXML());
    //   xdoc.Save(path);
    // }

    // public bool Load(string path)
    // {
    //   if (!File.Exists(path)) return false;
    //   XDocument xdoc = XDocument.Load(path);
    //   this.FromXML(xdoc.Root);
    //   return true;
    // }

    public static object CurrentConfig { get; set; }
    public static void Use(object config)
    {
      ArgumentNullException.ThrowIfNull(config);

      CurrentConfig = config;

      GameMain.LuaCs.Hook.Add("stop", (object[] args) =>
      {
        Save();
        return null;
      });

      GameMain.LuaCs.Hook.Add("roundEnd", (object[] args) =>
      {
        if (SaveEveryRound) Save();
        return null;
      });

    }

  }
}