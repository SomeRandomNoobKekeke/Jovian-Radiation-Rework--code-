using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;


using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace JovianRadiationRework
{
  public static class TypeCrawler
  {
    public static Dictionary<Type, bool> PrimitiveTypes = new Dictionary<Type, bool>(){
      {typeof(int), true},
      {typeof(bool), true},
      {typeof(string), true},
      {typeof(float), true},
      {typeof(double), true},
      {typeof(Color), true},
    };

    public static Dictionary<Type, bool> AllowedComplexTypes = new Dictionary<Type, bool>(){
      {typeof(Settings), true},
      {typeof(VanillaSettings), true},
      {typeof(ModSettings), true},
      {typeof(ProgressSettings), true},
    };

    // I wanted to make abstract method, but it seems needlesly complex
    // and i'll need more specific methods on top of that anyway,
    // and there's only 3 of them so i'll just make them
    public static void PrintProps(object o, string offset = "")
    {
      List<PropertyInfo> primitive = new List<PropertyInfo>();
      List<PropertyInfo> complex = new List<PropertyInfo>();

      foreach (PropertyInfo pi in o.GetType().GetProperties(AccessTools.all))
      {
        if (Attribute.IsDefined(pi, typeof(IgnoreAttribute))) continue;

        if (PrimitiveTypes.GetValueOrDefault(pi.PropertyType))
        {
          primitive.Add(pi);
        }

        if (AllowedComplexTypes.GetValueOrDefault(pi.PropertyType))
        {
          complex.Add(pi);
        }
      }

      foreach (PropertyInfo pi in complex)
      {
        Mod.Log($"{offset}{pi.Name}:", Color.Violet);
        PrintProps(pi.GetValue(o), offset + "    ");
      }

      foreach (PropertyInfo pi in primitive)
      {
        // BaroDev (Wide)
#if CLIENT
        string s = Mod.WrapInColor(pi.GetValue(o), "128,255,255");
#elif SERVER
        string s = pi.GetValue(o).ToString();
#endif

        Mod.Log($"{offset}{pi.Name}  {s}  ({pi.PropertyType.Name})", Color.White);
      }
    }

    public static XElement ToXML(object o, XElement e)
    {
      List<PropertyInfo> primitive = new List<PropertyInfo>();
      List<PropertyInfo> complex = new List<PropertyInfo>();

      foreach (PropertyInfo pi in o.GetType().GetProperties(AccessTools.all))
      {
        if (Attribute.IsDefined(pi, typeof(IgnoreAttribute))) continue;
        if (Attribute.IsDefined(pi, typeof(IgnoreWriteAttribute))) continue;

        if (PrimitiveTypes.GetValueOrDefault(pi.PropertyType))
        {
          primitive.Add(pi);
        }

        if (AllowedComplexTypes.GetValueOrDefault(pi.PropertyType))
        {
          complex.Add(pi);
        }
      }

      foreach (PropertyInfo pi in complex)
      {
        e.Add(ToXML(pi.GetValue(o), new XElement(pi.Name)));
      }

      foreach (PropertyInfo pi in primitive)
      {
        e.Add(new XElement(pi.Name, pi.GetValue(o)));
      }

      return e;
    }

    public static void FromXML(object o, XElement e)
    {
      Dictionary<string, PropertyInfo> primitive = new Dictionary<string, PropertyInfo>();
      Dictionary<string, PropertyInfo> complex = new Dictionary<string, PropertyInfo>();

      foreach (PropertyInfo pi in o.GetType().GetProperties(AccessTools.all))
      {
        if (Attribute.IsDefined(pi, typeof(IgnoreAttribute))) continue;
        if (Attribute.IsDefined(pi, typeof(IgnoreReadAttribute))) continue;

        if (PrimitiveTypes.GetValueOrDefault(pi.PropertyType))
        {
          primitive[pi.Name] = pi;
        }

        if (AllowedComplexTypes.GetValueOrDefault(pi.PropertyType))
        {
          complex[pi.Name] = pi;
        }
      }

      foreach (XElement child in e.Elements())
      {
        if (complex.ContainsKey(child.Name.ToString()))
        {
          FromXML(complex[child.Name.ToString()].GetValue(o), child);
        }

        if (primitive.ContainsKey(child.Name.ToString()))
        {
          try
          {
            PropertyInfo pi = primitive[child.Name.ToString()];
            pi.SetValue(o, UltimateParser.Parse(pi.PropertyType, child.Value));
          }
          catch (Exception ex)
          {
            Mod.Error(ex);
          }
        }
      }
    }




  }
}