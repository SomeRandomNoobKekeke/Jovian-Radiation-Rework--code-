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

    public class TreeClimber
    {
      public Hints Current;
      public void AttachTo(Hints node) => Current = node;
      public Hint TryMoveTo(string bruh)
      {
        if (Current is null)
        {
          throw new Exception($"Can't climb to [{bruh}], climber not attached");
        }

        Hint hint = Current.FindClosest(bruh);

        if (hint is not null)
        {
          Current = hint;
        }

        return hint;
      }
    }


    /// <summary>
    /// Logic is this:  
    /// if there's more than 1 arg it will try to repair middle args first, 
    /// if it couldn't then it'll quit early  
    /// If last arg is incomplete it'll try to autocomplete  
    /// else If line ends in ' ' then it'll add first hint from the next level  
    /// else it'll cycle on this level
    /// </summary>
    public string AutoComplete(string fullString, int increment = 1)
    {
      if (Hints is null) return fullString;

      string[] splitCommand = ToolBox.SplitCommand(fullString);
      string[] args = splitCommand.Skip(1).ToArray();
      bool shouldStep = fullString.Last() == ' ';
      string commandName = splitCommand[0];

      if (args.Length == 0)
      {
        return shouldStep ? $"{commandName} {Hints.First()}" : commandName;
      }

      List<Hint> hints = new List<Hint>();

      string Result()
        => $"{commandName} {string.Join(' ', hints.Select(h => (h as Hint).Name))}";

      // Repair middle args
      if (args.Length > 1)
      {
        TreeClimber climber = new TreeClimber();
        climber.AttachTo(this.Hints);


        for (int i = 0; i < args.Length - 1; i++)
        {
          Hint hint = climber.TryMoveTo(args[i]);

          if (hint is null) return Result();

          hints.Add(hint);
        }
      }

      Hints lastHint = hints.LastOrDefault() ?? this.Hints;
      Hint directFind = lastHint.GetChild(args.Last());

      if (directFind is not null)
      {
        if (shouldStep)
        {
          hints.Add(directFind);

          if (directFind.HasChildren)
          {
            hints.Add(directFind.First());
          }
        }
        else
        {
          hints.Add(lastHint.Next(directFind));
        }
      }
      else
      {
        Hint autocompleted = lastHint.FindClosest(args.Last());
        if (autocompleted is not null)
        {
          hints.Add(autocompleted);
        }
      }


      return Result();
    }


    public AdvancedCommand(string name, string help, Action<string[]> onExecute, Hints hints = null, Func<string[][]> getValidArgs = null, bool isCheat = false) : base(name, help, onExecute, getValidArgs, isCheat)
    {
      Hints = hints;
    }
  }
}