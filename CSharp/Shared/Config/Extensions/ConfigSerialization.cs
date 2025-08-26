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
  public static class ConfigSerialization
  {
    public static string ToText(object config)
      => ConfigLogging.ToText(ConfigTraverse.GetFlatValues(config));

    public static XElement ToXML(object config)
    {
      // i think it's ok to throw here because you're supposed to use it as extension method on an instance
      ArgumentNullException.ThrowIfNull(config);

      XElement element = new XElement(config.GetType().Name);

      PropertyInfo[] props = ConfigTraverse.GetProps(config);

      foreach (PropertyInfo pi in props)
      {
        if (pi.PropertyType.IsAssignableTo(typeof(IConfig)))
        {
          IConfig subConfig = pi.GetValue(config) as IConfig;
          if (subConfig is null) continue;
          element.Add(ToXML(subConfig));
        }
      }

      foreach (PropertyInfo pi in props)
      {
        if (!pi.PropertyType.IsAssignableTo(typeof(IConfig)))
        {
          element.Add(XMLParser.Serialize(pi.GetValue(config), pi.Name));
        }
      }

      return element;
    }



    public static object FromXML(object config, XElement element)
    {
      if (config is null) return null;

      foreach (XElement child in element.Elements())
      {
        PropertyInfo pi = config.GetType().GetProperty(child.Name.ToString(), BindingFlags.Public | BindingFlags.Instance);
        if (pi is null) continue;

        if (pi.PropertyType.IsAssignableTo(typeof(IConfig)))
        {
          IConfig subConfig = (IConfig)pi.GetValue(config);
          if (subConfig is null)
          {
            subConfig = (IConfig)Activator.CreateInstance(pi.PropertyType);
            pi.SetValue(config, subConfig);
          }

          FromXML(subConfig, child);
        }
        else
        {
          pi.SetValue(config, XMLParser.Parse(child, pi.PropertyType));
        }
      }

      return config;
    }

    public static Func<string[][]> ToHints(object config)
    {
      if (config is null) return () => new string[][] { };
      return () => new string[][] { ConfigTraverse.GetFlat(config).Keys.ToArray() };
    }

    public static Hint ToAdvancedHints(object config)
    {
      Hint root = new Hint();

      if (config is null) return root;

      void scanHintsRec(Type T, Hint node)
      {
        PropertyInfo[] props = T.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        foreach (PropertyInfo pi in props)
        {
          if (pi.PropertyType.IsAssignableTo(typeof(IConfig)))
          {
            Hint subNode = new Hint(pi.Name);
            node.Children.Add(subNode);
            scanHintsRec(pi.PropertyType, subNode);
          }
        }

        foreach (PropertyInfo pi in props)
        {
          if (!pi.PropertyType.IsAssignableTo(typeof(IConfig)))
          {
            node.Children.Add(new Hint(pi.Name));
          }
        }
      }

      scanHintsRec(config.GetType(), root);

      return root;
    }
  }
}