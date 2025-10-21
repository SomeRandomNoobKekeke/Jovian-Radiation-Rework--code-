using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;

namespace BaroJunk_Config
{

  public partial class ConfigCore
  {
    public bool EqualsTo(ConfigCore other) => IsEqual(this, other);
    public static bool IsEqual(ConfigCore configA, ConfigCore configB)
      => Compare(configA, configB).Equals;


    public ConfigCompareResult CompareTo(ConfigCore other) => Compare(this, other);
    public static ConfigCompareResult Compare(ConfigCore configA, ConfigCore configB)
      => new ConfigCompareResult(configA, configB);


    /// <summary>
    /// Set everything to defaults
    /// </summary>
    public void Clear()
    {
      foreach (ConfigEntry entry in this.GetEntriesRec())
      {
        entry.Value = SimpleParser.Default.DefaultFor(entry.Type);
      }
    }

    /// <summary>
    /// Make sure all nested configs are not null
    /// </summary>
    public void Restore()
    {
      foreach (ConfigEntry entry in this.GetAllEntriesRec())
      {
        if (entry.IsConfig)
        {
          entry.Value ??= entry.Host.CreateDefaultForType(entry.Type).Target;
        }
      }
    }


    //CURSED what if config is husked?
    public void CopyTo(ConfigCore other)
    {
      foreach (var (key, entry) in this.GetAllFlat())
      {
        other.GetEntry(key).Value = entry.Value;
      }
    }

    // CURSED
    // public ConfigCore Copy()
    // {
    //   ConfigCore copy = new ConfigCore(Host.CreateDefaultForType(RawTarget.GetType()));
    //   this.CopyTo(copy);
    //   return copy;
    // }
  }

}