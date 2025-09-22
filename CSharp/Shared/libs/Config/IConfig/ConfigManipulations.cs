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

namespace BaroJunk
{

  public partial interface IConfig
  {
    public bool EqualsTo(IConfig other) => IsEqual(this, other);
    public static bool IsEqual(IConfig configA, IConfig configB)
      => Compare(configA, configB).Equals;


    public ConfigCompareResult CompareTo(IConfig other) => Compare(this, other);
    public static ConfigCompareResult Compare(IConfig configA, IConfig configB)
      => new ConfigCompareResult(configA, configB);


    /// <summary>
    /// Set everything to defaults
    /// </summary>
    public void Clear()
    {
      foreach (ConfigEntry entry in this.GetEntriesRec())
      {
        entry.Value = Parser.DefaultFor(entry.Type);
      }
    }

    /// <summary>
    /// Make sure all nested configs are not null
    /// </summary>
    public void Restore()
    {
      void RestoreRec(object o)
      {
        PropertyInfo[] props = o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

        foreach (PropertyInfo pi in props)
        {
          if (pi.PropertyType.IsAssignableTo(typeof(IConfig)))
          {
            if (pi.GetValue(o) is null)
            {
              pi.SetValue(o, Parser.DefaultFor(pi.PropertyType));
            }

            RestoreRec(pi.GetValue(o));
          }
        }
      }

      RestoreRec(this);
    }


    public void CopyTo(IConfig other)
    {
      foreach (var (key, entry) in this.GetAllFlat())
      {
        other.Get(key).Value = entry.Value;
      }
    }
    public IConfig Copy()
    {
      IConfig copy = (IConfig)Activator.CreateInstance(this.GetType());
      this.CopyTo(copy);
      return copy;
    }
  }

}