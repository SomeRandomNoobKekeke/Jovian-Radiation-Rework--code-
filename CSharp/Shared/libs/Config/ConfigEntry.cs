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
  //TODO should it really be structure?
  public struct ConfigEntry
  {
    public static ConfigEntry Empty = new ConfigEntry();

    public ConfigEntry this[string key] { get => Get(key); }
    public ConfigEntry Get(params string[] propPaths)
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

      ConfigEntry entry = new ConfigEntry(Value, names.First());
      foreach (string prop in names.Skip(1)) entry = entry[prop];

      return entry;
    }

    public PropertyInfo Property;
    public object Target;
    public object Value
    {
      get => Property?.GetValue(Target);
      set { if (IsValid) Property.SetValue(Target, value); }
    }

    public void SetValue(object newValue) => Value = newValue;
    public bool IsValid => Property is not null && Target is not null;

    public ConfigEntry() => (Target, Property) = (null, null);
    public ConfigEntry(object target, PropertyInfo property) => (Target, Property) = (target, property);
    public ConfigEntry(object target, string propName)
    {
      Target = target;
      if (propName is not null)
      {
        Property = target?.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
      }
      else
      {
        Property = null;
      }
    }
    public override string ToString() => $"{Target?.GetType().Name}.{Property?.Name}[{Value}]";
  }

  public static class ConfigEntryExtensions
  {
    public static string ToText(this IEnumerable<ConfigEntry> collection, string separator = ", ", string edge = "")
      => $"[{edge}{String.Join(separator, collection.Select(ce => ce.ToString()))}{edge}]";

    public static string ToText(this Dictionary<string, ConfigEntry> flat)
    {
      StringBuilder sb = new StringBuilder();

      sb.Append("{\n");
      foreach (string key in flat.Keys)
      {
        sb.Append($"    {key}: [{flat[key]}],\n");
      }
      sb.Append("}");

      return sb.ToString();
    }
  }


}