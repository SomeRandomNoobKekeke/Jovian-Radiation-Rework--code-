using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;



namespace BetterRadiation
{
  public partial class Mod : IAssemblyPlugin
  {
    public static void addCommands()
    {
      DebugConsole.Commands.Add(new DebugConsole.Command("setradiationspeed", "default is 100", (string[] args) =>
      {
        if (args.Length > 0 && float.TryParse(args[0], out float speed))
        {
          if (GameMain.GameSession?.Map?.Radiation != null)
          {
            GameMain.GameSession.Map.Radiation.Params.RadiationStep = Math.Max(0, speed);
          }
        }
      }));
    }

    public static void removeCommands()
    {
      DebugConsole.Commands.RemoveAll(c => c.Names.Contains("setradiationspeed"));
    }
  }
}