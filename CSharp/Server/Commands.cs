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
    public void AddProjSpecificCommands()
    {
      AddedCommands.Add(new DebugConsole.Command("rad_serv_amount", "", (string[] args) =>
      {
        if (args.Length == 0 || !float.TryParse(args[0], out float amount)) return;

        GameMain.GameSession.Map.Radiation.Amount = amount;
      }));

      //TODO i need more sync net events for this
      // AddedCommands.Add(new DebugConsole.Command("rad_amount", "rad_amount [value]", (string[] args) =>
      // {
      //   if (args.Length != 0 && GameMain.GameSession?.Map?.Radiation != null)
      //   {
      //     if (float.TryParse(args[0], out float amount))
      //     {
      //       GameMain.GameSession.Map.Radiation.Amount = amount;
      //     }
      //   }

      //   Mod.Log($"Radiation.Amount = {GameMain.GameSession?.Map?.Radiation.Amount}");
      // }));
    }
  }
}