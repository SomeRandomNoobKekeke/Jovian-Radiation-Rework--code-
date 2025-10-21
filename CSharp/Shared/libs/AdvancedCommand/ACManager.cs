using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using MoonSharp.Interpreter;
using Microsoft.Xna.Framework;

namespace BaroJunk
{
  public class ACManager
  {
    public static string ModHookId => Assembly.GetExecutingAssembly().GetName().Name;
    public static bool HooksInstalled;
    public static void MakeSureHooksInstalled()
    {
      if (HooksInstalled) return;
      HooksInstalled = true;

      GameMain.LuaCs.Hook.Patch(ModHookId + ".ACAutoComplete",
        typeof(DebugConsole).GetMethod("AutoComplete", AccessTools.all),
        DebugConsole_AutoComplete_Prefix
      );
    }

    public static DynValue DebugConsole_AutoComplete_Prefix(object instance, LuaCsHook.ParameterTable ptable)
    {
      // Already handled in some other mod
      if (ptable.PreventExecution) return null;

      string command = (string)ptable["command"];
      int increment = (int)ptable["increment"];

      string[] splitCommand = ToolBox.SplitCommand(command);
      string[] args = splitCommand.Skip(1).ToArray();

      if (args.Length > 0 || (splitCommand.Length > 0 && command.Last() == ' '))
      {
        DebugConsole.Command matchingCommand = DebugConsole.commands.Find(c => c.Names.Contains(splitCommand[0].ToIdentifier()));

        if (matchingCommand is AdvancedCommand ac && ac.HasCustomAutocomplete)
        {
          try
          {
            ptable.ReturnValue = ac.AutoComplete(command, increment);
          }
          catch (Exception e)
          {
            LuaCsLogger.LogMessage($"Couldn't autocomplete command: [{e.Message}]", Color.Yellow * 0.8f, Color.Yellow);
          }
          ptable.PreventExecution = true; return null;
        }
      }

      ptable.PreventExecution = false; return null;
    }
  }
}