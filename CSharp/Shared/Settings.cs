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
using Barotrauma.Networking;
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

    //[NetworkSerialize]
    public class ModSettings //: INetSerializableStruct
    {
      [XmlAttribute] public float WaterRadiationBlockPerMeter { get; set; } = 0.6f;
      [XmlAttribute] public float RadiationDamage { get; set; } = 0.037f;
      [XmlAttribute] public float TooMuchEvenForMonsters { get; set; } = 300;
      [XmlAttribute] public float HuskRadiationResistance { get; set; } = 0.5f;
      [XmlAttribute] public float RadiationToColor { get; set; } = 0.001f;
      [XmlAttribute] public float WorldProgressStepDuration { get; set; } = 10.0f;
      [XmlAttribute] public int WorldProgressMaxStepsPerRound { get; set; } = 5;
      [XmlAttribute] public bool UseVanillaRadiation { get; set; } = false;

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

      public void apply()
      {
        vanilla.apply();
      }

      public void print()
      {
        log("Mod Radiation Settings:", Color.DeepPink);
        foreach (PropertyInfo prop in typeof(ModSettings).GetProperties())
        {
          log($"{prop} = {prop.GetValue(modSettings)}");
        }

        log("Vanilla Radiation Settings:", Color.DeepPink);
        foreach (PropertyInfo prop in typeof(MyRadiationParams).GetProperties())
        {
          log($"{prop} = {prop.GetValue(vanilla)}");
        }
      }

      // doesn't work >:(
      // public static void encode(Settings s, IWriteMessage msg)
      // {
      //   msg.WriteNetSerializableStruct(s.modSettings);
      //   msg.WriteNetSerializableStruct(s.vanilla);
      // }

      // public static void decode(Settings s, IReadMessage msg)
      // {
      //   s.modSettings = INetSerializableStruct.Read<ModSettings>(msg);
      //   s.vanilla = INetSerializableStruct.Read<MyRadiationParams>(msg);
      // }

      public static void encode(Settings s, IWriteMessage msg)
      {
        // mod settings 
        msg.WriteSingle(s.modSettings.HuskRadiationResistance);
        msg.WriteSingle(s.modSettings.RadiationDamage);
        msg.WriteSingle(s.modSettings.RadiationToColor);
        msg.WriteSingle(s.modSettings.TooMuchEvenForMonsters);
        msg.WriteSingle(s.modSettings.WaterRadiationBlockPerMeter);

        // vanilla
        msg.WriteInt32(s.vanilla.MinimumOutpostAmount);
        msg.WriteInt32(s.vanilla.CriticalRadiationThreshold);

        msg.WriteSingle(s.vanilla.StartingRadiation);
        msg.WriteSingle(s.vanilla.RadiationStep);
        msg.WriteSingle(s.vanilla.AnimationSpeed);
        msg.WriteSingle(s.vanilla.RadiationDamageDelay);
        msg.WriteSingle(s.vanilla.RadiationDamageAmount);
        msg.WriteSingle(s.vanilla.MaxRadiation);
        msg.WriteSingle(s.vanilla.BorderAnimationSpeed);

        msg.WriteString(s.vanilla.RadiationAreaColor);
        msg.WriteString(s.vanilla.RadiationBorderTint);
      }

      public static void decode(Settings s, IReadMessage msg)
      {
        // mod settings 
        s.modSettings.HuskRadiationResistance = msg.ReadSingle();
        s.modSettings.RadiationDamage = msg.ReadSingle();
        s.modSettings.RadiationToColor = msg.ReadSingle();
        s.modSettings.TooMuchEvenForMonsters = msg.ReadSingle();
        s.modSettings.WaterRadiationBlockPerMeter = msg.ReadSingle();

        // vanilla
        s.vanilla.MinimumOutpostAmount = msg.ReadInt32();
        s.vanilla.CriticalRadiationThreshold = msg.ReadInt32();

        s.vanilla.StartingRadiation = msg.ReadSingle();
        s.vanilla.RadiationStep = msg.ReadSingle();
        s.vanilla.AnimationSpeed = msg.ReadSingle();
        s.vanilla.RadiationDamageDelay = msg.ReadSingle();
        s.vanilla.RadiationDamageAmount = msg.ReadSingle();
        s.vanilla.MaxRadiation = msg.ReadSingle();
        s.vanilla.BorderAnimationSpeed = msg.ReadSingle();

        s.vanilla.RadiationAreaColor = msg.ReadString();
        s.vanilla.RadiationBorderTint = msg.ReadString();
      }
    }

  }
}