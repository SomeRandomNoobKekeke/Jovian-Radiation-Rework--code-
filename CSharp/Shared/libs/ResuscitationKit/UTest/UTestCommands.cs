using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using Barotrauma;
namespace BaroJunk
{
  public class UTestCommands
  {
    public static List<DebugConsole.Command> AddedCommands = new List<DebugConsole.Command>();
    //TODO what if multiple mods have this command?
    //TODO Why this method is the main entry point?
    public static void AddCommands()
    {
      AddedCommands.Add(new DebugConsole.Command("utest", "", UTest_Command, UTest_Hints));

      DebugConsole.Commands.InsertRange(0, AddedCommands);

      //TODO sneaky sneaky
      string lastCommand = ModStorage.Get<string>("lastUtestCommand");
      if (!string.IsNullOrEmpty(lastCommand))
      {
        DebugConsole.ExecuteCommand(lastCommand);
      }
    }

    //TODO remove hidden test from hints
    public static string[][] UTest_Hints()
      => new string[][] { UTestExplorer.TestNames.Append("all").Append("none").OrderBy(s => s.Length).ToArray() };

    public static int depth = 0;
    public static void UTest_Command(string[] args)
    {
      try
      {
        if (args.Length == 0)
        {
          ModStorage.Set("lastUtestCommand", null);
          UTestExplorer.PrintAllTests();
          return;
        }

        void RunTestPack(Type T)
        {
          List<UTestPack> results = UTestRunner.RunRecursive(T);

          foreach (UTestPack pack in results)
          {
            if (pack.NotEmpty) pack.Log();
          }
        }


        if (String.Equals(args[0], "none", StringComparison.OrdinalIgnoreCase))
        {
          ModStorage.Set("lastUtestCommand", null);
          return;
        }

        if (String.Equals(args[0], "all", StringComparison.OrdinalIgnoreCase))
        {
          ModStorage.Set("lastUtestCommand", "utest all");
          foreach (Type T in UTestExplorer.TestTree.Roots.Select(root => root.Type))
          {
            RunTestPack(T);
          }
          return;
        }

        Type start = UTestExplorer.TestByName.GetValueOrDefault(args[0]);

        if (start is null)
        {
          UTestLogger.Warning($"Can't find [{args[0]}] test");
          return;
        }

        ModStorage.Set("lastUtestCommand", $"utest {args[0]}");
        RunTestPack(start);
      }
      catch (Exception e) { UTestLogger.Warning($"utest failed with: {e.Message}"); }
    }

    public static void RemoveCommands()
    {
      AddedCommands.ForEach(c => DebugConsole.Commands.Remove(c));
      AddedCommands.Clear();
    }
  }
}