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

    public ConfigEntry this[string key] { get => Get(key); }
    public ConfigEntry Get(string propName, params string[] deeperProps)
    {
      ConfigEntry entry = new ConfigEntry(Value, propName);

      foreach (string prop in deeperProps) entry = entry.Get(prop);
      return entry;
    }

    public PropertyInfo Property;
    public object Target;
    public object Value
    {
      get => Property?.GetValue(Target);
      set { if (IsValid) Property.SetValue(Target, value); }
    }
    public bool IsValid => Property is not null && Target is not null;

    public ConfigEntry() => (Target, Property) = (null, null);
    public ConfigEntry(object target, PropertyInfo property) => (Target, Property) = (target, property);
    public ConfigEntry(object target, string propName)
    {
      Target = target;
      Property = target?.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
    }
    public override string ToString() => $"{Target?.GetType().Name}.{Property?.Name}";
  }

  // cringe
  public static class ConfigEntryExtensions
  {
    public static string ToText(this IEnumerable<ConfigEntry> collection, string separator = ", ", string edge = "")
      => $"[{edge}{String.Join(separator, collection.Select(ce => ce.ToString()))}{edge}]";
  }


}