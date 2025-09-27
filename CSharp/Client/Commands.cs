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
      AddedCommands.Add(new DebugConsole.Command("rad_printmodel", "", (string[] args) =>
      {
        Mod.Logger.Log(CurrentModel);
      }));

      AddedCommands.Add(new DebugConsole.Command("rad_amount", "", (string[] args) =>
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

        Mod.Logger.Log($"Radiation amount: {GameMain.GameSession.Map.Radiation.Amount}/{GameMain.GameSession.Map?.Width}");
      }));
    }

  }
}