using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using Barotrauma;
namespace ResuscitationKit
{
  public class UTestCommands
  {
    public static List<DebugConsole.Command> AddedCommands = new List<DebugConsole.Command>();
    public static void AddCommands()
    {
      AddedCommands.Add(new DebugConsole.Command("utest", "", UTest_Command, UTest_Hints));

      DebugConsole.Commands.InsertRange(0, AddedCommands);
    }

    public static string[][] UTest_Hints()
      => new string[][] { UTestExplorer.TestNames.Append("all").ToArray() };

    public static int depth = 0;
    public static void UTest_Command(string[] args)
    {
      if (args.Length == 0)
      {
        UTestExplorer.PrintAllTests();
        return;
      }

      Type start = String.Equals(args[0], "all", StringComparison.OrdinalIgnoreCase) ?
        UTestExplorer.TestTree.RootType : UTestExplorer.TestByName.GetValueOrDefault(args[0]);

      if (start is null)
      {
        UTestLogger.Warning($"Can't find [{args[0]}] test");
        return;
      }

      try
      {
        UTestExplorer.TestTree.RunRecursive((test) =>
        {
          if (test == typeof(UTestPack)) return; // bruh
          UTestPack.Run(test).Log();
        }, start);
      }
      catch (Exception e) { UTestLogger.Warning($"utest failed with: {e.Message}"); };
    }

    public static void RemoveCommands()
    {
      AddedCommands.ForEach(c => DebugConsole.Commands.Remove(c));
      AddedCommands.Clear();
    }
  }
}