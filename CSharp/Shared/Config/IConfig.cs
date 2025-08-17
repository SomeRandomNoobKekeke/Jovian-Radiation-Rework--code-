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
    public ConfigEntry this[string key] { get => IConfigExtensions.Get(this, key); }


  }


  /// <summary>
  /// You can't use methods with default implementation from an interface on an object without casting it to that interface
  /// But you can do it with extention 
  /// </summary>
  public static class IConfigExtensions
  {
    // ConfigTraverse
    public static PropertyInfo[] GetProps(this IConfig config) => ConfigTraverse.GetProps(config);
    public static IEnumerable<string> GetPropNames(this IConfig config) => ConfigTraverse.GetPropNames(config);
    public static IEnumerable<ConfigEntry> GetEntries(this IConfig config) => ConfigTraverse.GetEntries(config);
    public static ConfigEntry Get(this IConfig config, params string[] propPaths) => ConfigTraverse.Get(config, propPaths);
    public static IEnumerable<ConfigEntry> GetPropsRec(this IConfig config) => ConfigTraverse.GetPropsRec(config);
    public static Dictionary<string, ConfigEntry> GetFlat(this IConfig config) => ConfigTraverse.GetFlat(config);
    public static Dictionary<string, object> GetFlatValues(this IConfig config) => ConfigTraverse.GetFlatValues(config);


    // ConfigSerialization
    public static bool IsEqual(this IConfig config, Object other) => ConfigSerialization.IsEqual(config, other);
    public static ConfigCompareResult Compare(this IConfig config, Object configB) => ConfigSerialization.Compare(config, configB);
    public static string ToText(this IConfig config) => ConfigSerialization.ToText(config);
    public static XElement ToXML(this IConfig config) => ConfigSerialization.ToXML(config);
    public static void FromXML(this IConfig config, XElement element) => ConfigSerialization.FromXML(config, element);
  }
}