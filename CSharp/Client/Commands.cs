global using BaroJunk;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Barotrauma.Networking;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static bool DoIHavePermissions()
      => GameMain.IsSingleplayer || GameMain.Client?.IsServerOwner == true || GameMain.Client?.HasPermission(ClientPermissions.ConsoleCommands) == true;

    public partial void AddCommandsProjSpecific()
    {
      AddedCommands.Add(new DebugConsole.Command("rad_printmodel", "", Rad_PrintModel_Command));
      AddedCommands.Add(new DebugConsole.Command("rad_amount", "", Rad_Amount_Command));
    }

    public static void Rad_PrintModel_Command(string[] args)
    {
      Mod.Logger.Log(CurrentModel);
    }

    public static void Rad_Amount_Command(string[] args)
    {
      if (GameMain.GameSession?.Map?.Radiation is null) return;

      if (args.Length > 0)
      {
        if (!DoIHavePermissions())
        {
          Mod.Logger.Log($"You don't have permissions");
          return;
        }

        if (GameMain.IsMultiplayer)
        {
          GameMain.Client.SendConsoleCommand($"rad_amount {args[0]}");
        }

        if (float.TryParse(args[0], out float amount))
        {
          GameMain.GameSession.Map.Radiation.Amount = amount;
        }
      }

      Mod.Logger.Log($"Rad front: [{GameMain.GameSession.Map.Radiation.Amount}] Current location: [{Level.Loaded.StartLocation.MapPosition.X}-{Level.Loaded.EndLocation.MapPosition.X}] Map width: [{GameMain.GameSession.Map?.Width}]");
    }

  }
}