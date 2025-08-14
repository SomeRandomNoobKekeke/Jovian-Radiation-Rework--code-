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
  /// <summary>
  /// Marker interface  
  /// It's required to detect nested configs without digging into complex types
  /// </summary>
  public interface IConfig
  {
    public ConfigEntry this[string key] { get => IConfigExtensions.Get(this, key); }

    public PropertyInfo[] Props
      => this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

    public IEnumerable<ConfigEntry> Entries
      => Props.Select(pi => new ConfigEntry(this, pi));

    public IEnumerable<string> PropNames
      => Props.Select(pi => pi.Name);

    public IEnumerable<ConfigEntry> PropsRec
    {
      get
      {
        foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
          if (!pi.PropertyType.IsAssignableTo(typeof(IConfig)))
          {
            yield return new ConfigEntry(this, pi);
          }
        }

        foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
          if (pi.PropertyType.IsAssignableTo(typeof(IConfig)))
          {
            IConfig nestedConfig = pi.GetValue(this) as IConfig;
            if (nestedConfig is null) continue;
            foreach (ConfigEntry entry in nestedConfig.PropsRec)
            {
              yield return entry;
            }
          }
        }
      }
    }

    public Dictionary<string, ConfigEntry> Flat
    {
      get
      {
        Dictionary<string, ConfigEntry> flat = new();

        void scanPropsRec(IConfig config, string path = null)
        {
          foreach (PropertyInfo pi in config.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
          {
            string newPath = path is null ? pi.Name : String.Join('.', path, pi.Name);

            if (pi.PropertyType.IsAssignableTo(typeof(IConfig)))
            {
              IConfig nestedConfig = pi.GetValue(config) as IConfig;
              if (nestedConfig is null) continue;
              scanPropsRec(nestedConfig, newPath);
            }
            else
            {
              flat[newPath] = new ConfigEntry(config, pi);
            }
          }
        }

        scanPropsRec(this);
        return flat;
      }
    }

    public Dictionary<string, object> FlatValues
      => Flat.ToDictionary(kp => kp.Key, kp => kp.Value.Value);

    public string ToText()
      => OtherConfigExtensions.ToText(FlatValues);




    public XElement ToXML()
    {
      XElement element = new XElement(this.GetType().Name);

      foreach (PropertyInfo pi in Props)
      {
        if (pi.PropertyType.IsAssignableTo(typeof(IConfig)))
        {
          IConfig subConfig = pi.GetValue(this) as IConfig;
          if (subConfig is null) continue;
          element.Add(subConfig.ToXML());
        }
      }

      foreach (PropertyInfo pi in Props)
      {
        if (!pi.PropertyType.IsAssignableTo(typeof(IConfig)))
        {
          element.Add(XMLParser.Serialize(pi.GetValue(this), pi.Name));
        }
      }

      return element;
    }



    public void FromXML(XElement element)
    {
      foreach (XElement child in element.Elements())
      {
        PropertyInfo pi = this.GetType().GetProperty(child.Name.ToString(), BindingFlags.Public | BindingFlags.Instance);
        if (pi is null) continue;

        if (pi.PropertyType.IsAssignableTo(typeof(IConfig)))
        {
          IConfig subConfig = (IConfig)pi.GetValue(this);
          if (subConfig is null)
          {
            subConfig = (IConfig)Activator.CreateInstance(pi.PropertyType);
            pi.SetValue(this, subConfig);
          }

          subConfig.FromXML(child);
        }
        else
        {
          pi.SetValue(this, XMLParser.Parse(child, pi.PropertyType));
        }
      }
    }

    public void Save(string path)
    {
      XDocument xdoc = new XDocument();
      xdoc.Add(this.ToXML());
      xdoc.Save(path);
    }

    public bool Load(string path)
    {
      if (!File.Exists(path)) return false;
      XDocument xdoc = XDocument.Load(path);
      this.FromXML(xdoc.Root);
      return true;
    }


    public bool IsEqual(IConfig other)
      => Compare(other).Equals;
    public IConfigCompareResult Compare(IConfig other)
      => new IConfigCompareResult(this, other);
  }


  /// <summary>
  /// Actually you can't use method from an interface with default implementation on an object without casting it to that interface
  /// But you can do it with extention 
  /// </summary>
  public static class IConfigExtensions
  {
    public static ConfigEntry Get(this IConfig config, params string[] propPaths)
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

      ConfigEntry entry = new ConfigEntry(config, names.First());
      foreach (string prop in names.Skip(1)) entry = entry[prop];

      return entry;
    }

    public static PropertyInfo[] GetProps(this IConfig config) => config.Props;
    public static IEnumerable<ConfigEntry> GetEntries(this IConfig config) => config.Entries;
    public static IEnumerable<string> GetPropNames(this IConfig config) => config.PropNames;
    public static IEnumerable<ConfigEntry> GetPropsRec(this IConfig config) => config.PropsRec;
    public static Dictionary<string, ConfigEntry> GetFlat(this IConfig config) => config.Flat;
    public static Dictionary<string, object> GetFlatValues(this IConfig config) => config.FlatValues;
    public static string ToText(this IConfig config) => config.ToText();
    public static XElement ToXML(this IConfig config) => config.ToXML();
    public static void FromXML(this IConfig config, XElement xml) => config.FromXML(xml);
    public static bool IsEqual(this IConfig config, IConfig other) => config.IsEqual(other);
    public static IConfigCompareResult Compare(this IConfig config, IConfig other) => config.Compare(other);
  }
}