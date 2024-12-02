using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Barotrauma.Networking;

namespace JovianRadiationRework
{
  public class SettingsManager
  {
    public static FlatView flatView => Settings.flatView;

    //TODO think how to protect it, making it a struct is tedious
    private Settings current; public Settings Current
    {
      get => current;
      set
      {
        current = value;
        OnSettingsChanged();
      }
    }

    public void OnSettingsChanged()
    {
      Current?.Apply();
    }

    public void SetProp(string deepName, object value)
    {
      flatView.Set(Current, deepName, value);
      OnSettingsChanged();
    }
    public object GetProp(string deepName) => flatView.Get(Current, deepName);
    public T GetProp<T>(string deepName) => flatView.Get<T>(Current, deepName);
    public bool HasProp(string deepName) => flatView.Has(deepName);

    public void Use(Settings s) => Current = s;
    public void Reset() => Current = new Settings();

    public void Print()
    {
      TypeCrawler.PrintProps(Current);
    }

    public void SaveTo(string path)
    {
      try
      {
        Current.Name ??= Path.GetFileNameWithoutExtension(path);
        XDocument doc = new XDocument();
        XElement root = TypeCrawler.ToXML(Current, new XElement("Settings"));
        doc.Add(root);
        doc.Save(path);
      }
      catch (Exception e)
      {
        Mod.Error(e);
      }
    }

    public void LoadFrom(string path) => Use(JustLoad(path));
    public Settings JustLoad(string path)
    {
      Settings next = new Settings();
      try
      {
        XDocument doc = XDocument.Load(path);
        XElement root = doc.Element("Settings");
        TypeCrawler.FromXML(next, root);
        next.Name ??= Path.GetFileNameWithoutExtension(path);
      }
      catch (Exception e)
      {
        Mod.Error(e);
      }

      return next;
    }

    public void Encode(IWriteMessage msg) => Encode(Current, msg);

    public void Decode(IReadMessage msg)
    {
      Decode(Current, msg);
      OnSettingsChanged();
    }

    public static void Encode(Settings s, IWriteMessage msg)
    {
      foreach (string key in flatView.Props.Keys)
      {
        NetManager.WriteObject(flatView.Get(s, key), msg);
      }
    }

    public static void Decode(Settings s, IReadMessage msg)
    {
      foreach (string key in flatView.Props.Keys)
      {
        flatView.Set(s, key, NetManager.ReadObject(flatView.Props[key].PropertyType, msg));
      }
    }

    public static bool Compare(Settings s1, Settings s2)
    {
      bool ok = true;

      foreach (string key in flatView.Props.Keys)
      {
        if (!Object.Equals(flatView.Get(s1, key), flatView.Get(s2, key)))
        {
          ok = false;
        }
      }

      return ok;
    }

    public static Dictionary<string, bool> DeepCompare(Settings s1, Settings s2)
    {
      Dictionary<string, bool> result = new Dictionary<string, bool>();

      foreach (string key in flatView.Props.Keys)
      {
        result[key] = Object.Equals(flatView.Get(s1, key), flatView.Get(s2, key));
      }

      return result;
    }

    public static List<string> Difference(Settings s1, Settings s2)
    {
      List<string> result = new List<string>();

      foreach (string key in flatView.Props.Keys)
      {
        if (!Object.Equals(flatView.Get(s1, key), flatView.Get(s2, key)))
        {
          result.Add(key);
        }
      }

      return result;
    }
  }
}