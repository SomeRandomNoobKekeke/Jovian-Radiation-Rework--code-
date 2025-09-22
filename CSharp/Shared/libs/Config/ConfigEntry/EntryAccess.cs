using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;

using Barotrauma;

namespace BaroJunk
{
  //TODO mb i should make it instance and add similar classes for access to dicts and lua tables 
  public static class EntryAccess
  {
    public static object GetProp(object target, string propPath)
    {
      ConfigEntry entry = GetEntry(target, propPath);
      return entry.Value;
    }

    public static void SetProp(object target, string propPath, object value)
    {
      ConfigEntry entry = GetEntry(target, propPath);
      entry.Value = value;
    }

    public static ConfigEntry GetEntry(object target, string propPath)
    {
      if (target is null || propPath is null) return ConfigEntry.Empty;

      string[] names = propPath.Split('.');
      if (names.Length == 0) return ConfigEntry.Empty;

      object o = target;

      foreach (string name in names.SkipLast(1))
      {
        if (o is null) return ConfigEntry.Empty;
        o = o.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance)?.GetValue(o);
      }

      if (o is null) return ConfigEntry.Empty;

      return new ConfigEntry(o, names.Last());
    }

    public static IEnumerable<ConfigEntry> GetEntries(object target)
      => target?.GetType()
          .GetProperties(BindingFlags.Public | BindingFlags.Instance)
          .Where(pi => !pi.PropertyType.IsAssignableTo(typeof(IConfig)))
          .Select(pi => new ConfigEntry(target, pi));

    public static IEnumerable<ConfigEntry> GetAllEntries(object target)
      => target?.GetType()
          .GetProperties(BindingFlags.Public | BindingFlags.Instance)
          .Select(pi => new ConfigEntry(target, pi));

    public static IEnumerable<ConfigEntry> GetEntriesRec(object config)
    {
      IEnumerable<ConfigEntry> entries = GetAllEntries(config);

      foreach (ConfigEntry entry in entries)
      {
        if (!entry.IsConfig) yield return entry;
      }

      foreach (ConfigEntry entry in entries)
      {
        if (entry.IsConfig)
        {
          IConfig nestedConfig = entry.Value as IConfig;
          if (nestedConfig is null) continue;

          foreach (ConfigEntry en in GetEntriesRec(nestedConfig))
          {
            yield return en;
          }
        }
      }
    }

    public static IEnumerable<ConfigEntry> GetAllEntriesRec(object config)
    {
      IEnumerable<ConfigEntry> entries = GetAllEntries(config);

      foreach (ConfigEntry entry in entries)
      {
        yield return entry;
      }

      foreach (ConfigEntry entry in entries)
      {
        if (entry.IsConfig)
        {
          IConfig nestedConfig = entry.Value as IConfig;
          if (nestedConfig is null) continue;

          foreach (ConfigEntry en in GetAllEntriesRec(nestedConfig))
          {
            yield return en;
          }
        }
      }
    }

    public static Dictionary<string, ConfigEntry> GetFlat(object config)
    {
      Dictionary<string, ConfigEntry> flat = new();

      void scanPropsRec(object cfg, string path = null)
      {
        foreach (ConfigEntry entry in GetAllEntries(cfg))
        {
          string newPath = path is null ? entry.Property.Name : String.Join('.', path, entry.Property.Name);

          if (entry.IsConfig)
          {
            IConfig nestedConfig = entry.Property.GetValue(cfg) as IConfig;
            if (nestedConfig is null) continue;
            scanPropsRec(nestedConfig, newPath);
          }
          else
          {
            flat[newPath] = entry;
          }
        }
      }

      scanPropsRec(config);

      return flat;
    }

    public static Dictionary<string, ConfigEntry> GetAllFlat(object config)
    {
      Dictionary<string, ConfigEntry> flat = new();

      void scanPropsRec(object cfg, string path = null)
      {
        foreach (ConfigEntry entry in GetAllEntries(cfg))
        {
          string newPath = path is null ? entry.Property.Name : String.Join('.', path, entry.Property.Name);

          flat[newPath] = entry;

          if (entry.IsConfig)
          {
            IConfig nestedConfig = entry.Property.GetValue(cfg) as IConfig;
            if (nestedConfig is null) continue;
            scanPropsRec(nestedConfig, newPath);
          }
        }
      }

      scanPropsRec(config);

      return flat;
    }

    public static Dictionary<string, object> GetFlatValues(object config)
      => GetFlat(config).ToDictionary(kp => kp.Key, kp => kp.Value.Value);

    public static Dictionary<string, object> GetAllFlatValues(object config)
      => GetAllFlat(config).ToDictionary(kp => kp.Key, kp => kp.Value.Value);
  }


}