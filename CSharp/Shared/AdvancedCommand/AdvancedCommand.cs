using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;

namespace JovianRadiationRework
{
  public class AdvancedCommand : DebugConsole.Command
  {
    public bool HasCustomAutocomplete => Hints is not null;
    public Hint Hints;

    public string AutoComplete(string command, int increment = 1)
    {
      string[] splitCommand = ToolBox.SplitCommand(command);
      string[] args = splitCommand.Skip(1).ToArray();

      Mod.LogArray(splitCommand);
      Mod.LogArray(args);

      return command;
    }


    public AdvancedCommand(string name, string help, Action<string[]> onExecute, Hint hints = null, Func<string[][]> getValidArgs = null, bool isCheat = false) : base(name, help, onExecute, getValidArgs, isCheat)
    {
      Hints = hints;
    }
  }
}