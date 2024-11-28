using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;


namespace JovianRadiationRework
{
  public class FlatView
  {
    public static Dictionary<Type, bool> PrimitiveTypes = new Dictionary<Type, bool>(){
      {typeof(int), true},
      {typeof(bool), true},
      {typeof(string), true},
      {typeof(float), true},
      {typeof(double), true},
      {typeof(Color), true},
    };

    // need this, or ScanPropsRec will go deep and eat all memory
    public static Dictionary<Type, bool> AllowedComplexTypes = new Dictionary<Type, bool>(){
      {typeof(Settings), true},
      {typeof(VanillaSettings), true},
      {typeof(FlatViewTest.TestNestedSettings), true},
    };

    public static Dictionary<Type, bool> IgnoredTypes = new Dictionary<Type, bool>(){
      {typeof(FlatView), true},
    };


    public Type TargetType;

    public SortedDictionary<string, PropertyInfo> Props = new SortedDictionary<string, PropertyInfo>();
    public bool Has(string name) => Props.ContainsKey(name);

    private Dictionary<string, PropertyInfo> ScanPropsRec(Type T, string baseName = "")
    {
      Dictionary<string, PropertyInfo> props = new Dictionary<string, PropertyInfo>();

      foreach (PropertyInfo pi in T.GetProperties(AccessTools.all))
      {
        if (IgnoredTypes.GetValueOrDefault(pi.PropertyType)) continue;

        string n = pi.Name;
        if (baseName != "") n = baseName + "." + n;

        if (PrimitiveTypes.GetValueOrDefault(pi.PropertyType))
        {
          props[n] = pi;
        }

        if (AllowedComplexTypes.GetValueOrDefault(pi.PropertyType))
        {
          Dictionary<string, PropertyInfo> deep = ScanPropsRec(pi.PropertyType, n);
          deep.ToList().ForEach(p => props[p.Key] = p.Value);
        }
      }

      return props;
    }

    public void ScanProps(Type T)
    {
      TargetType = T;
      Props.Clear();
      Dictionary<string, PropertyInfo> props = ScanPropsRec(T);
      props.ToList().ForEach(p => Props[p.Key] = p.Value);
    }

    public FlatView(Type T)
    {
      ScanProps(T);
    }

    public object Get(object obj, string deepName)
    {
      if (obj == null)
      {
        Mod.Info("obj == null");
        return null;
      }

      if (deepName == null)
      {
        Mod.Info("deepName == null");
        return null;
      }

      string[] names = deepName.Split('.');

      foreach (string name in names)
      {
        if (obj == null)
        {
          Mod.Info("obj == null");
          return null;
        }
        PropertyInfo pi = obj.GetType().GetProperty(name, AccessTools.all);

        if (pi == null)
        {
          Mod.Info("PropertyInfo == null");
          return null;
        }

        obj = pi.GetValue(obj);
      }

      return obj;
    }

    public T Get<T>(object obj, string deepName)
    {
      return (T)Get(obj, deepName);
    }

    public void Set(object obj, string deepName, object value)
    {
      if (deepName == null)
      {
        Mod.Info("deepName == null");
        return;
      }

      string[] names = deepName.Split('.');

      foreach (string name in names.SkipLast(1))
      {
        if (obj == null)
        {
          Mod.Info("obj == null");
          return;
        }

        PropertyInfo pi = obj.GetType().GetProperty(name, AccessTools.all);

        if (pi == null)
        {
          Mod.Info("PropertyInfo == null");
          return;
        }

        obj = pi.GetValue(obj);
      }

      try
      {
        PropertyInfo pi = obj.GetType().GetProperty(names.Last(), AccessTools.all);
        if (value.GetType() == typeof(string) && pi.PropertyType != typeof(string))
        {
          value = UltimateParser.Parse(pi.PropertyType, (string)value);
        }
        pi.SetValue(obj, value);
      }
      catch (Exception e)
      {
        Mod.Error($"FlatView.Set({obj}, \"{deepName}\", {value})\n{e.Message}");
      }
    }
  }
}