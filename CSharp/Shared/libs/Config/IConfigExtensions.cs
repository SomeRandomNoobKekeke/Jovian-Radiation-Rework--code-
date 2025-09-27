using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace BaroJunk
{

  public static class IConfigExtensions
  {
    public static IConfig Self(this IConfig config) => config;
    public static ConfigSettings Settings(this IConfig config) => config.Settings;
    public static string GetName(this IConfig config) => config.Name;

    public static void OnPropChanged(this IConfig config, Action<string, object> action)
      => config.OnPropChanged(action);
    public static void EnableAutoSaving(this IConfig config) => config.Settings.AutoSave = true;


    public static IConfigEntry Get(this IConfig config, string entryPath) => config.Get(entryPath);
    public static IEnumerable<IConfigEntry> GetEntries(this IConfig config) => config.Entries;
    public static IEnumerable<IConfig> GetSubConfigs(this IConfig config) => config.SubConfigs;


    public static string ToText(this IConfig config) => config.ToText();
    public static XElement ToXML(this IConfig config) => config.ToXML();
    public static void FromXML(this IConfig config, XElement element) => config.FromXML(element);




    public static SimpleResult LoadSave(this IConfig config, string path) => config.LoadSave(path);
    public static SimpleResult Save(this IConfig config, string path) => config.Save(path);
    public static SimpleResult Load(this IConfig config, string path) => config.Load(path);




    public static bool EqualsTo(this IConfig config, IConfig other) => config.EqualsTo(other);
    public static ConfigCompareResult CompareTo(this IConfig config, IConfig other) => config.CompareTo(other);
    public static void Clear(this IConfig config) => config.Clear();
    public static void Restore(this IConfig config) => config.Restore();
    public static void Copy(this IConfig config) => config.Copy();
    public static void CopyTo(this IConfig config, IConfig other) => config.CopyTo(other);
  }

}