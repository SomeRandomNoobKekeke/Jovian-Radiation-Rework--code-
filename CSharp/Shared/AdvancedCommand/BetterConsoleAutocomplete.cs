using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using HarmonyLib;
using Barotrauma;

namespace JovianRadiationRework
{
  public class BetterConsoleAutocomplete
  {
    public static void Patch(Harmony harmony)
    {
      harmony.Patch(
        original: typeof(DebugConsole).GetMethod("AutoComplete", AccessTools.all),
        prefix: new HarmonyMethod(typeof(BetterConsoleAutocomplete).GetMethod("DebugConsole_AutoComplete_Prefix"))
      );

      harmony.Patch(
        original: typeof(DebugConsole).GetMethod("AutoComplete", AccessTools.all),
        postfix: new HarmonyMethod(typeof(BetterConsoleAutocomplete).GetMethod("DebugConsole_AutoComplete_Postfix"))
      );
    }

    public static bool DebugConsole_AutoComplete_Prefix(ref string __result, string command, int increment = 1)
    {
      string[] splitCommand = ToolBox.SplitCommand(command);
      string[] args = splitCommand.Skip(1).ToArray();

      if (args.Length > 0 || (splitCommand.Length > 0 && command.Last() == ' '))
      {
        DebugConsole.Command matchingCommand = DebugConsole.commands.Find(c => c.Names.Contains(splitCommand[0].ToIdentifier()));

        if (matchingCommand is AdvancedCommand ac && ac.HasCustomAutocomplete)
        {
          __result = ac.AutoComplete(command, increment);
          return false;
        }
        else
        {
          return true;
        }
      }

      return false;
    }

    public static void DebugConsole_AutoComplete_Postfix(ref string __result, string command, int increment = 1)
    {
      Mod.Log($"command: [{command}], increment: [{increment}]");
      Mod.Log(__result);
    }

  }
}