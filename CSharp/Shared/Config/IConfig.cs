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
  /// <summary>
  /// Marker interface
  /// </summary>
  public interface IConfig
  {
    public static IConfig Current
    {
      get => ConfigManager.CurrentConfig as IConfig;
      set => ConfigManager.CurrentConfig = value;
    }

    public static string SavePath
    {
      get => ConfigManager.SavePath;
      set => ConfigManager.SavePath = value;
    }

    public ConfigEntry this[string key] { get => IConfigExtensions.Get(this, key); }
  }


  /// <summary>
  /// You can't use methods with default implementation from an interface on an object without casting it to that interface
  /// But you can do it with extention 
  /// </summary>
  public static class IConfigExtensions
  {
    public static void SetAsCurrent(this IConfig config) => IConfig.Current = config;
    public static void SetSavePath(this IConfig config, string path) => IConfig.SavePath = path;
    public static string GetSavePath(this IConfig config) => IConfig.SavePath;

    // ConfigTraverse
    public static PropertyInfo[] GetProps(this IConfig config) => ConfigTraverse.GetProps(config);
    public static IEnumerable<string> GetPropNames(this IConfig config) => ConfigTraverse.GetPropNames(config);
    public static IEnumerable<ConfigEntry> GetEntries(this IConfig config) => ConfigTraverse.GetEntries(config);
    public static ConfigEntry Get(this IConfig config, params string[] propPaths) => ConfigTraverse.Get(config, propPaths);
    public static IEnumerable<ConfigEntry> GetPropsRec(this IConfig config) => ConfigTraverse.GetPropsRec(config);
    public static Dictionary<string, ConfigEntry> GetFlat(this IConfig config) => ConfigTraverse.GetFlat(config);
    public static Dictionary<string, object> GetFlatValues(this IConfig config) => ConfigTraverse.GetFlatValues(config);


    // ConfigSerialization
    public static bool IsEqual(this IConfig config, Object other) => ConfigComparison.IsEqual(config, other);
    public static ConfigCompareResult Compare(this IConfig config, Object configB) => ConfigComparison.Compare(config, configB);
    public static string ToText(this IConfig config) => ConfigSerialization.ToText(config);
    public static XElement ToXML(this IConfig config) => ConfigSerialization.ToXML(config);
    public static void FromXML(this IConfig config, XElement element) => ConfigSerialization.FromXML(config, element);


    public static ConfigSaver.ConfigSaverResult Save(this IConfig config, string path = null) => ConfigSaver.Save(path);
    public static ConfigSaver.ConfigSaverResult Load(this IConfig config, string path = null) => ConfigSaver.Load(path);


    public static void NetSync(this IConfig config) => ConfigNetworking.NetSync();
  }
}