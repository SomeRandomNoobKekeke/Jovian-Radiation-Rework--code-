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
      DebugConsole.Commands.Add(new DebugConsole.Command("rad_serv_step", "", (string[] args) =>
      {
        int steps = 1;
        if (args.Length > 0) int.TryParse(args[0], out steps);
        GameMain.GameSession?.Map?.Radiation?.OnStep(steps * settings.modSettings.Progress.WorldProgressStepDuration);
      }));

      DebugConsole.Commands.Add(new DebugConsole.Command("rad_serv_set", "", (string[] args) =>
      {
        if (args.Length == 0 || !float.TryParse(args[0], out float amount)) return;

        GameMain.GameSession.Map.Radiation.Amount = amount;
      }));
    }

    public static void removeCommands()
    {
      DebugConsole.Commands.RemoveAll(c => c.Names.Contains("rad_serv_step"));
      DebugConsole.Commands.RemoveAll(c => c.Names.Contains("rad_serv_set"));
    }
  }
}