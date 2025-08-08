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

namespace JovianRadiationRework
{
  public static class ConfigTraverse
  {
    public static BindingFlags PublicInstance = BindingFlags.Public | BindingFlags.Instance;


    public static IEnumerable<ConfigEntry> PropsFirst(object config)
    {
      if (config is null) yield break;

      foreach (PropertyInfo pi in config.GetType().GetProperties(PublicInstance))
      {
        if (!pi.PropertyType.IsAssignableTo(typeof(IConfig)))
        {
          yield return new ConfigEntry(pi, config);
        }
      }

      foreach (PropertyInfo pi in config.GetType().GetProperties(PublicInstance))
      {
        if (pi.PropertyType.IsAssignableTo(typeof(IConfig)))
        {
          IConfig nestedConfig = pi.GetValue(config) as IConfig;
          foreach (ConfigEntry entry in PropsFirst(nestedConfig))
          {
            yield return entry;
          }
        }
      }
    }

    public static IEnumerable<ConfigEntry> DepthFirst(object config)
    {
      if (config is null) yield break;

      foreach (PropertyInfo pi in config.GetType().GetProperties(PublicInstance))
      {
        if (pi.PropertyType.IsAssignableTo(typeof(IConfig)))
        {
          IConfig nestedConfig = pi.GetValue(config) as IConfig;
          foreach (ConfigEntry entry in DepthFirst(nestedConfig))
          {
            yield return entry;
          }
        }
      }

      foreach (PropertyInfo pi in config.GetType().GetProperties(PublicInstance))
      {
        if (!pi.PropertyType.IsAssignableTo(typeof(IConfig)))
        {
          yield return new ConfigEntry(pi, config);
        }
      }
    }
  }
}