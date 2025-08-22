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
  public static class ConfigComparison
  {
    public static bool IsEqual(Object configA, Object configB)
      => Compare(configA, configB).Equals;
    public static ConfigCompareResult Compare(Object configA, Object configB)
      => new ConfigCompareResult(configA, configB);

    public static void Clear(object config)
    {
      foreach (ConfigEntry entry in ConfigTraverse.GetFlat(config).Values)
      {
        entry.SetValue(Parser.DefaultFor(entry.Property.PropertyType));
      }
    }
  }
}