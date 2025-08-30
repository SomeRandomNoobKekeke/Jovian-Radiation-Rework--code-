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
  public static class ConfigTraverse
  {
    public static PropertyInfo[] GetProps(object config)
      => config.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

    public static IEnumerable<string> GetPropNames(object config)
      => GetProps(config).Select(pi => pi.Name);

    public static IEnumerable<ConfigEntry> GetEntries(object config)
      => GetProps(config).Select(pi => new ConfigEntry(config, pi));

    public static ConfigEntry Get(object config, params string[] propPaths)
    {
      List<string> names = new List<string>();

      if (propPaths is not null)
      {
        foreach (string path in propPaths)
        {
          if (path is null) names.Add(null);
          else names.AddRange(path.Split('.'));
        }
      }

      if (names.Count == 0) return ConfigEntry.Empty;

      ConfigEntry entry = new ConfigEntry(config, names.First());
      foreach (string prop in names.Skip(1)) entry = entry[prop];

      return entry;
    }

    public static IEnumerable<ConfigEntry> GetPropsRec(object config)
    {
      PropertyInfo[] props = GetProps(config);

      foreach (PropertyInfo pi in props)
      {
        if (!pi.PropertyType.IsAssignableTo(typeof(IConfig)))
        {
          yield return new ConfigEntry(config, pi);
        }
      }

      foreach (PropertyInfo pi in props)
      {
        if (pi.PropertyType.IsAssignableTo(typeof(IConfig)))
        {
          IConfig nestedConfig = pi.GetValue(config) as IConfig;
          if (nestedConfig is null) continue;
          foreach (ConfigEntry entry in GetPropsRec(nestedConfig))
          {
            yield return entry;
          }
        }
      }
    }

    public static Dictionary<string, ConfigEntry> GetFlat(object config)
    {
      Dictionary<string, ConfigEntry> flat = new();

      void scanPropsRec(object cfg, string path = null)
      {
        foreach (PropertyInfo pi in GetProps(cfg))
        {
          string newPath = path is null ? pi.Name : String.Join('.', path, pi.Name);

          if (pi.PropertyType.IsAssignableTo(typeof(IConfig)))
          {
            IConfig nestedConfig = pi.GetValue(cfg) as IConfig;
            if (nestedConfig is null) continue;
            scanPropsRec(nestedConfig, newPath);
          }
          else
          {
            flat[newPath] = new ConfigEntry(cfg, pi);
          }
        }
      }

      scanPropsRec(config);

      return flat;
    }

    public static Dictionary<string, object> GetFlatValues(object config)
      => GetFlat(config).ToDictionary(kp => kp.Key, kp => kp.Value.Value);
  }
}