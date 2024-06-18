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
      DebugConsole.Commands.Add(new DebugConsole.Command("radinfo", "", (string[] args) =>
      {
        log("Radiation Settings:", Color.DeepPink);
        foreach (PropertyInfo prop in typeof(Settings).GetProperties())
        {
          if (prop.Name == "vanilla") continue;
          log($"{prop} = {prop.GetValue(settings)}");
        }

        log("Vanilla Radiation Settings:", Color.DeepPink);
        foreach (PropertyInfo prop in typeof(MyRadiationParams).GetProperties())
        {
          log($"{prop} = {prop.GetValue(settings.vanilla)}");
        }
      }));
    }

    public static void removeCommands()
    {
      DebugConsole.Commands.RemoveAll(c => c.Names.Contains("radinfo"));
    }
  }
}