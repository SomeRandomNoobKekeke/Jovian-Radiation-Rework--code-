using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.IO;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Barotrauma.Extensions;
using Barotrauma.Networking;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace JovianRadiationRework
{
  public partial class Mod
  {
    public static string ModSettingsFolder = "ModSettings\\";
    public static string SettingsFolder = "ModSettings\\Jovian Radiation Rework\\";
    public static string SettingsFileName = "Settings.json";
    public static string NewSettingsFileName = "new Settings.json";
    public static string PresetsFolder = "Presets";
    public static string DefaultPreset = "Default.json";

    public static void createFolders()
    {
      if (!Directory.Exists(ModSettingsFolder)) Directory.CreateDirectory(ModSettingsFolder);
      if (!Directory.Exists(SettingsFolder)) Directory.CreateDirectory(SettingsFolder);
      if (!Directory.Exists(Path.Combine(SettingsFolder, PresetsFolder))) Directory.CreateDirectory(Path.Combine(SettingsFolder, PresetsFolder));
    }

    public partial struct Settings
    {
      public ModSettings modSettings { get; set; } = new ModSettings();
      public MyRadiationParams vanilla { get; set; } = new MyRadiationParams();

      public string Author { get; set; } = "";
      public string Description { get; set; } = "";

      public Settings() { }

      public static Settings load(string fileName = "")
      {
        if (fileName == "") fileName = SettingsFileName;
        else if (!fileName.EndsWith(".json")) fileName = $"{fileName}.json";

        Settings newSettings = new Settings();

        bool found = tryLoadPreset(Path.Combine(SettingsFolder, fileName));
        if (!found) found = tryLoadPreset(Path.Combine(SettingsFolder, PresetsFolder, fileName));
        if (!found) found = tryLoadPreset(Path.Combine(meta.ModDir, PresetsFolder, fileName));
        if (!found) tryLoadPreset(Path.Combine(meta.ModDir, PresetsFolder, DefaultPreset));

        bool tryLoadPreset(string file)
        {
          if (!File.Exists(file)) return false;
          try { newSettings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(file)); }
          catch (Exception e) { log(e, Color.Orange); return false; }
          return true;
        }

        return newSettings;
      }

      public static void save(Settings s, string fileName = "")
      {
        string path = "";
        if (fileName == "") path = Path.Combine(SettingsFolder, NewSettingsFileName);
        else
        {
          if (!fileName.EndsWith(".json")) fileName = $"{fileName}.json";
          path = Path.Combine(SettingsFolder, PresetsFolder, fileName);
        }

        try
        {
          File.WriteAllText(path, json(s, true));
        }
        catch (Exception e) { log(e, Color.Orange); }
      }

      public void moveObsoleteSettingsToNewPath()
      {
        modSettings.Step.WorldProgressMaxStepsPerRound = (int)modSettings.WorldProgressMaxStepsPerRound;
        modSettings.WorldProgressMaxStepsPerRound = null;

        modSettings.Step.WorldProgressStepDuration = (float)modSettings.WorldProgressStepDuration;
        modSettings.WorldProgressStepDuration = null;
      }

      public void apply()
      {
        moveObsoleteSettingsToNewPath();
        vanilla.apply();
        modSettings.ActualColor = XMLExtensions.ParseColor(modSettings.AmbienceColor);
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
        log($"Author: {Author}", Color.DeepPink);
        log($"Description: {Description}");
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
        msg.WriteSingle(s.modSettings.WaterRadiationBlockPerMeter);
        msg.WriteSingle(s.modSettings.RadiationDamage);
        msg.WriteSingle(s.modSettings.RadiationSlowDown);
        msg.WriteSingle(s.modSettings.TooMuchEvenForMonsters);
        msg.WriteSingle(s.modSettings.FractionOfRadiationBlockedInSub);
        msg.WriteSingle(s.modSettings.HuskRadiationResistance);
        msg.WriteSingle(s.modSettings.RadiationToAmbienceBrightness);
        msg.WriteSingle(s.modSettings.MaxAmbienceBrightness);
        msg.WriteSingle(s.modSettings.Step.WorldProgressStepDuration);

        msg.WriteInt32(s.modSettings.Step.WorldProgressMaxStepsPerRound);
        msg.WriteBoolean(s.modSettings.UseVanillaRadiation);
        msg.WriteBoolean(s.modSettings.KeepSurroundingOutpostsAlive);
        msg.WriteString(s.modSettings.AmbienceColor);


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
        s.modSettings.WaterRadiationBlockPerMeter = msg.ReadSingle();
        s.modSettings.RadiationDamage = msg.ReadSingle();
        s.modSettings.RadiationSlowDown = msg.ReadSingle();
        s.modSettings.TooMuchEvenForMonsters = msg.ReadSingle();
        s.modSettings.FractionOfRadiationBlockedInSub = msg.ReadSingle();
        s.modSettings.HuskRadiationResistance = msg.ReadSingle();
        s.modSettings.RadiationToAmbienceBrightness = msg.ReadSingle();
        s.modSettings.MaxAmbienceBrightness = msg.ReadSingle();
        s.modSettings.Step.WorldProgressStepDuration = msg.ReadSingle();

        s.modSettings.Step.WorldProgressMaxStepsPerRound = msg.ReadInt32();
        s.modSettings.UseVanillaRadiation = msg.ReadBoolean();
        s.modSettings.KeepSurroundingOutpostsAlive = msg.ReadBoolean();
        s.modSettings.AmbienceColor = msg.ReadString();

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