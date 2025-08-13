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
  public interface IConfig : IPropsContainer
  {
    public ConfigEntry this[string key] { get => new ConfigEntry(this, key); }

    public PropertyInfo[] Props
      => this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

    public IEnumerable<ConfigEntry> Entries
      => Props.Select(pi => new ConfigEntry(this, pi));

    public IEnumerable<string> PropNames
      => Props.Select(pi => pi.Name);
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

  }
}