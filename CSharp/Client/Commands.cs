using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static void addCommands()
    {
      DebugConsole.Commands.Add(new DebugConsole.Command("rad_info", "", (string[] args) =>
      {
        log($"Current location irradiation: {CurrentLocationRadiationAmount()}");

        settings.print();
      }));

      DebugConsole.Commands.Add(new DebugConsole.Command("rad_step", "", (string[] args) =>
      {
        GameMain.GameSession?.Map?.Radiation?.OnStep();

        if (GameMain.IsMultiplayer)
        {
          DebugConsole.ExecuteCommand("rad_serv_step");
        }
      }));

      DebugConsole.Commands.Add(new DebugConsole.Command("rad_set", "rad_set amount", (string[] args) =>
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

      DebugConsole.Commands.Add(new DebugConsole.Command("rad_reset", "resets settings to default", (string[] args) =>
      {
        settings = new Settings();
        settings.apply();
        Settings.save(settings);
        log("Radiation settings reset");
        if (GameMain.IsMultiplayer) Settings.sync(settings);
      }));

      DebugConsole.Commands.Add(new DebugConsole.Command("rad_load", "load settings", (string[] args) =>
      {
        settings = Settings.load();
        settings.apply();
        Settings.save(settings);
        log("Radiation settings loaded");
        if (GameMain.IsMultiplayer) Settings.sync(settings);
      }));


      DebugConsole.Commands.Add(new DebugConsole.Command("rad", "rad setting value", (string[] args) =>
      {
        if (args.Length == 0) { log("rad setting value"); return; }

        PropertyInfo prop = null;
        object target = null;

        if (typeof(MyRadiationParams).GetProperty(args[0]) != null)
        {
          prop = typeof(MyRadiationParams).GetProperty(args[0]);
          target = settings.vanilla;
        }

        if (typeof(ModSettings).GetProperty(args[0]) != null)
        {
          prop = typeof(ModSettings).GetProperty(args[0]);
          target = settings.modSettings;
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
      }, () => new string[][] {
        typeof(MyRadiationParams).GetProperties().Select(p => p.Name).Concat(typeof(ModSettings).GetProperties().Select(p => p.Name)).OrderBy(s=>s).ToArray()
      }));
    }

    public static void removeCommands()
    {
      DebugConsole.Commands.RemoveAll(c => c.Names.Contains("rad_info"));
      DebugConsole.Commands.RemoveAll(c => c.Names.Contains("rad_step"));
      DebugConsole.Commands.RemoveAll(c => c.Names.Contains("rad_set"));
      DebugConsole.Commands.RemoveAll(c => c.Names.Contains("rad_reset"));
      DebugConsole.Commands.RemoveAll(c => c.Names.Contains("rad_load"));
      DebugConsole.Commands.RemoveAll(c => c.Names.Contains("rad"));
    }

    public static void permitCommands(Identifier command, ref bool __result)
    {
      if (command.Value == "rad_info") __result = true;
      if (command.Value == "rad_step" && HasPermissions) __result = true;
      if (command.Value == "rad_set" && HasPermissions) __result = true;
      if (command.Value == "rad_reset" && HasPermissions) __result = true;
      if (command.Value == "rad_load" && HasPermissions) __result = true;
      if (command.Value == "rad" && HasPermissions) __result = true;

      if (command.Value == "rad_serv_step" && HasPermissions) __result = true;
      if (command.Value == "rad_serv_set" && HasPermissions) __result = true;
    }
  }
}