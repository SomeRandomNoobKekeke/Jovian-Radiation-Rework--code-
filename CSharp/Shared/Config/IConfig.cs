using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;

namespace JovianRadiationRework
{
  /// <summary>
  /// Marker interface  
  /// It's required to detect nested configs without digging into complex types
  /// </summary>
  public interface IConfig
  {
    public ConfigEntry this[string key] { get => new ConfigEntry(this, key); }

    public PropertyInfo[] Props
      => this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

    public IEnumerable<ConfigEntry> Entries
      => Props.Select(pi => new ConfigEntry(this, pi));

    public IEnumerable<string> PropNames
      => Props.Select(pi => pi.Name);

    public IEnumerable<ConfigEntry> PropsRec
    {
      get
      {
        foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
          if (!pi.PropertyType.IsAssignableTo(typeof(IConfig)))
          {
            yield return new ConfigEntry(this, pi);
          }
        }

        foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
          if (pi.PropertyType.IsAssignableTo(typeof(IConfig)))
          {
            IConfig nestedConfig = pi.GetValue(this) as IConfig;
            if (nestedConfig is null) continue;
            foreach (ConfigEntry entry in nestedConfig.PropsRec)
            {
              yield return entry;
            }
          }
        }
      }
    }

  }


  /// <summary>
  /// Actually you can't use method from an interface with default implementation on an object without casting it to that interface
  /// But you can do it with extention 
  /// </summary>
  public static class IConfigExtensions
  {
    public static ConfigEntry Get(this IConfig config, string propName, params string[] deeperProps)
    {
      ConfigEntry entry = new ConfigEntry(config, propName);
      foreach (string prop in deeperProps) entry = entry[prop];
      return entry;
    }

    public static PropertyInfo[] GetProps(this IConfig config) => config.Props;
    public static IEnumerable<ConfigEntry> GetEntries(this IConfig config) => config.Entries;
    public static IEnumerable<string> GetPropNames(this IConfig config) => config.PropNames;
    public static IEnumerable<ConfigEntry> GetPropsRec(this IConfig config) => config.PropsRec;


  }
}