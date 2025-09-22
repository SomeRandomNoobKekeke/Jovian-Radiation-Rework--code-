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

namespace BaroJunk
{
  public class ConfigEntry : IConfigEntry
  {
    public static ConfigEntry Empty => new ConfigEntry();

    public string Name => Property?.Name;
    public Type Type => Property?.PropertyType;
    public PropertyInfo Property;
    public object Target;
    public bool IsValid => Property is not null && Target is not null;
    public object Value
    {
      get => Property?.GetValue(Target);
      set { if (IsValid) Property.SetValue(Target, value); }
    }

    public IConfigEntry this[string key] { get => Get(key); }
    public IConfigEntry Get(string entryPath)
      => EntryAccess.GetEntry(Value, entryPath);
    public IEnumerable<IConfigEntry> Entries
      => EntryAccess.GetAllEntries(Value);

    public bool IsConfig => Property.PropertyType.IsAssignableTo(typeof(IConfig));

    public ConfigEntry() { }
    public ConfigEntry(object target, PropertyInfo property) => (Target, Property) = (target, property);
    public ConfigEntry(object target, string propName)
    {
      Target = target;
      if (propName is not null)
      {
        Property = target?.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
      }
    }

    public override bool Equals(object obj)
    {
      if (obj is not ConfigEntry other) return false;
      return other.Property == Property && other.Target == Target;
    }

    public override string ToString() => $"{Target?.GetType().Name}.{Property?.Name} [{Value}]";
  }





}