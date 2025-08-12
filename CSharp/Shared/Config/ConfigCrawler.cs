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
  public class ConfigCrawler
  {
    public object Target { get; set; }
    public bool Attached => Target is not null;

    ConfigCrawler this[string key]
    {
      get => new ConfigEntry(this, key);
      set
      {
        PropertyInfo pi = this.GetType().GetProperty(key, BindingFlags.Instance | BindingFlags.Public);
        if (pi is null) return;
        pi.SetValue(this, value);
      }
    }

    public void MoveTo(object newTarget) => Target = newTarget;
    public ConfigCrawler Get(string propName)
    {
      if (!Attached) return null;

      MoveTo(Value.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.Instance)?.GetValue(Value));
      return this;
    }

    public ConfigCrawler(object target) => Target = target;
  }
}