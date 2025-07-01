using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.IO;

using Barotrauma;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;

namespace JovianRadiationRework
{
  public partial class Mod
  {
    public static List<DebugConsole.Command> AddedCommands = new List<DebugConsole.Command>();
    public static partial void AddCommands();

    public static void RemoveCommands()
    {
      AddedCommands.ForEach(c => DebugConsole.Commands.Remove(c));
      AddedCommands.Clear();
    }

    public static void PermitCommands(Identifier command, ref bool __result)
    {
      if (AddedCommands.Any(c => c.Names.Contains(command.Value))) __result = true;
    }
  }
}