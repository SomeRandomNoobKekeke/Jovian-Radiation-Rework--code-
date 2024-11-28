using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.IO;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public void AddProjSpecificCommands()
    {
      AddedCommands.Add(new DebugConsole.Command("rad", "rad variable [value]", (string[] args) =>
      {
        if (args.Length == 0)
        {
          Log("rad variable [value]");
          return;
        }

        string name = args.ElementAtOrDefault(0);
        string value = args.ElementAtOrDefault(1);

        if (value != null)
        {
          settingsManager.SetProp(name, value);
        }

        Log($"{settingsManager.GetProp(name).GetType().Name} {settingsManager.GetProp(name)}");
      }, () => new string[][] { Settings.flatView.Props.Keys.ToArray() }));


      AddedCommands.Add(new DebugConsole.Command("rad_runtest", "rad test", (string[] args) =>
      {
        if (args.Length > 0)
        {
          Test.Run(args[0]);
        }
      }, () =>
      {
        Assembly CallingAssembly = Assembly.GetAssembly(typeof(Test));

        string[] allTest = CallingAssembly.GetTypes()
          .Where(T => T.IsSubclassOf(typeof(Test)))
          .Select(T => T.Name).Append("all").ToArray();

        return new string[][] { allTest };
      }));
    }
  }
}