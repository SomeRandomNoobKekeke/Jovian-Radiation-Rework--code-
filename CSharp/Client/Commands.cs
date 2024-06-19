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
        if (args.Length == 0 || !float.TryParse(args[0], out float amount)) return;

        GameMain.GameSession.Map.Radiation.Amount = amount;

        if (GameMain.IsMultiplayer)
        {
          DebugConsole.ExecuteCommand($"rad_serv_set {args[0]}");
        }
      }));

      // here was 
      DebugConsole.Commands.Add(new DebugConsole.Command("rad", "rad setting value", (string[] args) =>
      {
        if (args.Length < 2) { log("rad setting value"); return; }

        if (GameMain.IsSingleplayer)
        {
          switch (args[0].Trim())
          {
            case "WaterRadiationBlockPerMeter":
              if (float.TryParse(args[1], out float f)) settings.modSettings.WaterRadiationBlockPerMeter = f;
              break;
            case "RadiationDamage":
              if (float.TryParse(args[1], out f)) settings.modSettings.RadiationDamage = f;
              log(settings.modSettings.RadiationDamage);
              break;
            case "TooMuchEvenForMonsters":
              if (float.TryParse(args[1], out f)) settings.modSettings.TooMuchEvenForMonsters = f;
              break;
            case "HuskRadiationResistance":
              if (float.TryParse(args[1], out f)) settings.modSettings.HuskRadiationResistance = f;
              break;
            case "RadiationToColor":
              if (float.TryParse(args[1], out f)) settings.modSettings.RadiationToColor = f;
              break;

            case "CriticalRadiationThreshold":
              if (float.TryParse(args[1], out float f)) settings.modSettings.WaterRadiationBlockPerMeter = f;
              break;
            case "MinimumOutpostAmount":
              if (float.TryParse(args[1], out f)) settings.modSettings.RadiationDamage = f;
              log(settings.modSettings.RadiationDamage);
              break;

            case "TooMuchEvenForMonsters":
              if (float.TryParse(args[1], out f)) settings.modSettings.TooMuchEvenForMonsters = f;
              break;
            case "HuskRadiationResistance":
              if (float.TryParse(args[1], out f)) settings.modSettings.HuskRadiationResistance = f;
              break;
            case "RadiationToColor":
              if (float.TryParse(args[1], out f)) settings.modSettings.RadiationToColor = f;
              break;
          }
        }
      }, () => new string[][] {
        typeof(MyRadiationParams).GetProperties().Select(p => p.Name).ToArray(),
        typeof(ModSettings).GetProperties().Select(p => p.Name).ToArray()
      }));
    }

    public static void removeCommands()
    {
      DebugConsole.Commands.RemoveAll(c => c.Names.Contains("rad_info"));
      DebugConsole.Commands.RemoveAll(c => c.Names.Contains("rad_step"));
      DebugConsole.Commands.RemoveAll(c => c.Names.Contains("rad_set"));
      DebugConsole.Commands.RemoveAll(c => c.Names.Contains("rad"));
    }

    public static void permitCommands(Identifier command, ref bool __result)
    {
      if (command.Value == "rad_info") __result = true;
      if (command.Value == "rad_step" && HasPermissions) __result = true;
      if (command.Value == "rad_set" && HasPermissions) __result = true;
      if (command.Value == "rad" && HasPermissions) __result = true;

      if (command.Value == "rad_serv_step" && HasPermissions) __result = true;
      if (command.Value == "rad_serv_set" && HasPermissions) __result = true;

    }
  }
}