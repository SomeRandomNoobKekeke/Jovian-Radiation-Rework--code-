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

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public List<DebugConsole.Command> AddedCommands = new List<DebugConsole.Command>();
    public void AddCommands()
    {
      AddProjSpecificCommands();

      AddedCommands.Add(new DebugConsole.Command("rad_print", "", (string[] args) =>
      {
        Mod.Instance.settingsManager.Print();
      }));

      DebugConsole.Commands.InsertRange(0, AddedCommands);
    }

    public void RemoveCommands()
    {
      AddedCommands.ForEach(c => DebugConsole.Commands.RemoveAll(which => which.Names.Contains(c.Names[0])));

      AddedCommands.Clear();
      AddedCommands = null;
    }
  }
}