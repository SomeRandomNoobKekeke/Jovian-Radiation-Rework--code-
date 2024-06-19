using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Barotrauma.Extensions;
namespace JovianRadiationRework
{
  public partial class Mod
  {
    public static string ModSettingsFolder = "ModSettings\\";
    public static string SettingsFolder = "ModSettings\\Jovian Radiation Rework\\";
    public static string SettingsFileName = "Settings.xml";

    public static XmlSerializer serializer;

    public static void createFolders()
    {
      if (!Directory.Exists(ModSettingsFolder)) Directory.CreateDirectory(ModSettingsFolder);
      if (!Directory.Exists(SettingsFolder)) Directory.CreateDirectory(SettingsFolder);
    }

    public struct ModMetadata
    {
      public string ModVersion { get; set; } = "1.0.0";
      public string ModName { get; set; } = "Jovian Radiation Rework";
      public string ModDir { get; set; } = "";

      public ModMetadata() { }
    }


    public struct ModSettings
    {
      [XmlAttribute] public float WaterRadiationBlockPerMeter { get; set; } = 0.6f;
      [XmlAttribute] public float RadiationDamage { get; set; } = 0.037f;
      [XmlAttribute] public float TooMuchEvenForMonsters { get; set; } = 300;
      [XmlAttribute] public float HuskRadiationResistance { get; set; } = 0.5f;
      [XmlAttribute] public float RadiationToColor { get; set; } = 0.001f;

      public ModSettings() { }
    }

    public partial struct Settings
    {
      public ModSettings modSettings { get; set; } = new ModSettings();
      public MyRadiationParams vanilla { get; set; } = new MyRadiationParams();

      public Settings() { }

      public static Settings load(string path = "")
      {
        if (path == "") path = Path.Combine(SettingsFolder, SettingsFileName);
        if (serializer == null) { serializer = new XmlSerializer(typeof(Settings)); }

        Settings newSettings = new Settings();

        if (File.Exists(path))
        {
          try
          {
            using (var reader = new StreamReader(path))
            using (var xmlreader = XmlReader.Create(reader))
            {
              newSettings = (Settings)serializer.Deserialize(xmlreader);
            }
          }
          catch (Exception e) { err(e); }
        }

        return newSettings;
      }

      public static void save(Settings s, string path = "")
      {
        if (path == "") path = Path.Combine(SettingsFolder, SettingsFileName);
        if (serializer == null) { serializer = new XmlSerializer(typeof(Settings)); }

        try
        {
          using (var writer = new StreamWriter(path))
          using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true }))
          {
            serializer.Serialize(xmlWriter, s);
          }
        }
        catch (Exception e) { err(e); }
      }
    }

  }
}