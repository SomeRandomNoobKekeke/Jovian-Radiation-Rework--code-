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
        current.Apply();
      }
    }

    public void SetProp(string deepName, object value)
    {
      flatView.Set(Current, deepName, value);
      Current.Apply();
    }
    public object GetProp(string deepName) => flatView.Get(Current, deepName);
    public T GetProp<T>(string deepName) => flatView.Get<T>(Current, deepName);
    public bool HasProp(string deepName) => flatView.Has(deepName);

    public void Use(Settings s) => Current = s;
    public void Reset() => Current = new Settings();

    public void Print()
    {
      foreach (string key in flatView.Props.Keys)
      {
        Mod.Log($"{key} = {flatView.Get(Current, key)}");
      }
    }

    public void SaveTo(string path)
    {
      try
      {
        XDocument doc = new XDocument();
        XElement root = new XElement("Settings");
        foreach (string key in flatView.Props.Keys)
        {
          root.Add(new XElement(key, flatView.Get(Current, key)));
        }
        doc.Add(root);
        doc.Save(path);
      }
      catch (Exception e)
      {
        Mod.Error(e);
      }
    }

    public void LoadFrom(string path)
    {
      try
      {
        XDocument doc = XDocument.Load(path);
        XElement root = doc.Element("Settings");
        foreach (XElement e in root.Elements())
        {
          flatView.Set(Current, e.Name.ToString(), e.Value);
        }
      }
      catch (Exception e)
      {
        Mod.Error(e);
      }
    }
  }
}