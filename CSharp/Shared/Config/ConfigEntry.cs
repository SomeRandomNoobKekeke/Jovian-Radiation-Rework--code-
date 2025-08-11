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
  public struct ConfigEntry
  {
    public static ConfigEntry Empty = new ConfigEntry();

    public PropertyInfo Property;
    public object Target;
    public object Value
    {
      get => Property?.GetValue(Target);
      set => Property?.SetValue(Target, value);
    }
    public bool Valid => Property is not null && Target is not null;

    public ConfigEntry Get(string propName) => new ConfigEntry(Target, propName);


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
      Property = target?.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
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