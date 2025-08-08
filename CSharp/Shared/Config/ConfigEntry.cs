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
  public struct ConfigEntry : IEntry
  {
    public PropertyInfo Property;
    public object Target;
    public object Value => Property?.GetValue(Target);

    public ConfigEntry Get(string propName)
      => new ConfigEntry(Target, propName);

    public ConfigEntry()
    {
      Property = null;
      Target = null;
    }
    public ConfigEntry(PropertyInfo property, object target)
    {
      Property = property;
      Target = target;
    }
    public ConfigEntry(object target, string propName)
    {
      Target = target;
      Property = target?.GetType().GetProperty(propName);
    }
    public override string ToString() => $"{Target.GetType().Name}.{Property.Name}";
  }

  // cringe
  public static class ConfigEntryExtensions
  {
    public static string ToText(this IEnumerable<ConfigEntry> collection, string separator = ", ", string edge = "")
      => $"[{edge}{String.Join(separator, collection.Select(ce => ce.ToString()))}{edge}]";
  }


}