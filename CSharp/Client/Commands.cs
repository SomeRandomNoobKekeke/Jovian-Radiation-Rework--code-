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
    public static partial void AddCommands()
    {
      AddedCommands.Add(new AdvancedCommand("bruh", "", Bruh_Command,
        new Hints(
          new Hint("ko",
            new Hint("ko1"),
            new Hint("ko2"),
            new Hint("ko3")
          ),
          new Hint("ju",
            new Hint("ju1"),
            new Hint("ju2")
          ),
          new Hint("be",
            new Hint("be1"),
            new Hint("be2",
              new Hint("bububu")
            )
          )
        )
      ));

      DebugConsole.Commands.InsertRange(0, AddedCommands);
    }

    public static void Bruh_Command(string[] args)
    {
      Mod.Log("Bruh");
    }
  }
}