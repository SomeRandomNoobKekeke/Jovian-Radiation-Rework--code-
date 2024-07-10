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

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static List<DebugConsole.Command> addedCommands = new List<DebugConsole.Command>();

    public static void addCommands()
    {
      if (addedCommands == null) addedCommands = new List<DebugConsole.Command>();

      addedCommands.Add(new DebugConsole.Command("rad_info", "", (string[] args) =>
      {
        settings.print();
        log($"Current location irradiation: {CurrentLocationRadiationAmount()}", Color.Yellow);
        log($"Current Radiation.Amount: {GameMain.GameSession?.Map?.Radiation.Amount}", Color.Yellow);
      }));


      addedCommands.Add(new DebugConsole.Command("rad_step", "", (string[] args) =>
      {
        float steps = 1;
        if (args.Length > 0) float.TryParse(args[0], out steps);

        GameMain.GameSession?.Map?.Radiation?.OnStep(steps * settings.modSettings.Progress.WorldProgressStepDuration);

        if (GameMain.IsMultiplayer)
        {
          DebugConsole.ExecuteCommand("rad_serv_step");
        }
      }));

      addedCommands.Add(new DebugConsole.Command("rad_set", "rad_set amount", (string[] args) =>
      {
        if (args.Length == 0)
        {
          log($"Radiation.Amount = {GameMain.GameSession.Map.Radiation.Amount}");
          return;
        }

        if (!float.TryParse(args[0], out float amount)) return;

        GameMain.GameSession.Map.Radiation.Amount = amount;

        if (GameMain.IsMultiplayer)
        {
          DebugConsole.ExecuteCommand($"rad_serv_set {args[0]}");
        }
      }));

      addedCommands.Add(new DebugConsole.Command("rad_reset", "resets settings to default", (string[] args) =>
      {
        settings = Settings.load(DefaultPreset);
        settings.apply();
        Settings.save(settings);
        log("Radiation settings reset to default");
        if (GameMain.IsMultiplayer) Settings.sync(settings);
      }));

      addedCommands.Add(new DebugConsole.Command("rad_load", "load settings, press tab to cycle through presets", (string[] args) =>
      {
        string filename = "";
        if (args.Length > 0) filename = args[0];

        settings = Settings.load(filename);
        settings.apply();
        Settings.save(settings);
        log("Radiation settings loaded");
        if (GameMain.IsMultiplayer) Settings.sync(settings);
      }, () =>
      {
        try
        {
          return new string[][] {
            Directory.GetFiles(Path.Combine(SettingsFolder, PresetsFolder),"*.json")
            .Concat(Directory.GetFiles(Path.Combine(meta.ModDir, PresetsFolder),"*.json"))
            .Select(p=>Path.GetFileNameWithoutExtension(p)).ToArray()
          };
        }
        catch (Exception e) { err(e); }

        return new string[][] { };
      }));

      addedCommands.Add(new DebugConsole.Command("rad_save", "save settings as", (string[] args) =>
      {
        string filename = "";
        if (args.Length > 0) filename = args[0];

        Settings.save(settings, filename);
        log($"Radiation settings saved as {filename}");
      }));


      addedCommands.Add(new DebugConsole.Command("rad", "rad setting value", (string[] args) =>
      {
        if (args.Length == 0) { log("rad setting value"); return; }

        PropertyInfo prop = null;
        object target = null;

        if (typeof(VanillaRadiationParams).GetProperty(args[0]) != null)
        {
          prop = typeof(VanillaRadiationParams).GetProperty(args[0]);
          target = settings.vanilla;
        }

        if (typeof(ModSettings).GetProperty(args[0]) != null)
        {
          prop = typeof(ModSettings).GetProperty(args[0]);
          target = settings.modSettings;
        }

        if (typeof(ProgressSettings).GetProperty(args[0]) != null)
        {
          prop = typeof(ProgressSettings).GetProperty(args[0]);
          target = settings.modSettings.Progress;
        }

        if (prop != null)
        {
          if (args.Length == 1)
          {
            log($"{prop} {prop.GetValue(target)}");
            return;
          }

          try
          {
            if (prop.PropertyType == typeof(float)) prop.SetValue(target, float.Parse(args[1]));
            if (prop.PropertyType == typeof(int)) prop.SetValue(target, int.Parse(args[1]));
            if (prop.PropertyType == typeof(string)) prop.SetValue(target, args[1]);
            if (prop.PropertyType == typeof(bool)) prop.SetValue(target, bool.Parse(args[1]));

            settings.apply();
            Settings.save(settings);
            if (GameMain.IsMultiplayer) Settings.sync(settings);
          }
          catch (Exception e) { err(e); log("wat???"); }
        }
        else log("no such setting");
      }, Settings.getAllProps));


      addedCommands.ForEach(c => DebugConsole.Commands.Add(c));
    }

    public static void removeCommands()
    {
      addedCommands.ForEach((c => DebugConsole.Commands.RemoveAll(which => which.Names.Contains(c.Names[0]))));

      addedCommands.Clear();
      addedCommands = null;
    }

    public static void permitCommands(Identifier command, ref bool __result)
    {
      if (command.Value == "rad_info") __result = true;
      if (command.Value == "rad_step" && HasPermissions) __result = true;
      if (command.Value == "rad_set" && HasPermissions) __result = true;
      if (command.Value == "rad_reset" && HasPermissions) __result = true;
      if (command.Value == "rad_load" && HasPermissions) __result = true;
      if (command.Value == "rad_save") __result = true; // saved locally without syncing
      if (command.Value == "rad" && HasPermissions) __result = true;

      if (command.Value == "rad_serv_step" && HasPermissions) __result = true;
      if (command.Value == "rad_serv_set" && HasPermissions) __result = true;
    }
  }
}