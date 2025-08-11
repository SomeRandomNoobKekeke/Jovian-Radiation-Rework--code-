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
    ConfigEntry this[string key]
    {
      get => new ConfigEntry(this, key);
      set
      {
        PropertyInfo pi = this.GetType().GetProperty(key, BindingFlags.Instance | BindingFlags.Public);
        if (pi is null) return;
        pi.SetValue(this, value);
      }
    }


  }


  /// <summary>
  /// Actually you can't use method from an interface with default implementation on an object without casting it to that interface
  /// But you can do it with extention 
  /// </summary>
  public static class IConfigExtensions
  {
    public static ConfigEntry Get(this IConfig config, string propName) => new ConfigEntry(config, propName);
  }
}