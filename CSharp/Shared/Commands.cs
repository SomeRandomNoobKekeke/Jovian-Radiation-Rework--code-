global using BaroJunk;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;


namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public List<DebugConsole.Command> AddedCommands = new List<DebugConsole.Command>();

    public partial void AddCommandsProjSpecific();

    public void AddCommands()
    {
      AddCommandsProjSpecific();

      DebugConsole.Commands.InsertRange(0, AddedCommands);
    }

    public void RemoveCommands()
    {
      AddedCommands.ForEach(c => DebugConsole.Commands.Remove(c));
      AddedCommands.Clear();
    }
  }
}