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
    public Hints Hints;

    public string AutoComplete(string command, int increment = 1)
    {
      string[] splitCommand = ToolBox.SplitCommand(command);
      string[] args = splitCommand.Skip(1).ToArray();

      bool shouldStep = command.Last() == ' ';



      List<Hints> hints = new();
      hints.Add(Hints);

      string Pack()
        => $"{splitCommand[0]} {string.Join(' ', hints.Skip(1).Select(h => (h as Hint).Name))}";

      for (int i = 0; i < args.Length - 1; i++)
      {
        Hint hint = hints[i].FindClosest(args[i]);

        if (hint is null) return Pack();
        hints.Add(hint);
      }

      Hint autocompleted = hints.Last().FindClosest(args.LastOrDefault());

      if (shouldStep)
      {
        hints.Add(autocompleted);

        Hint next = autocompleted.First();
        if (next is not null) hints.Add(next);
      }
      else
      {
        hints.Add(hints.Last().Next(autocompleted));
      }

      return Pack();
    }


    public AdvancedCommand(string name, string help, Action<string[]> onExecute, Hints hints = null, Func<string[][]> getValidArgs = null, bool isCheat = false) : base(name, help, onExecute, getValidArgs, isCheat)
    {
      Hints = hints;
    }
  }
}