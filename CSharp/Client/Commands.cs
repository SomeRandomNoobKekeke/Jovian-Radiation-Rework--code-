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
using System.IO;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static bool DoIHavePermissions()
      => GameMain.IsSingleplayer || GameMain.Client?.IsServerOwner == true || GameMain.Client?.HasPermission(ClientPermissions.ConsoleCommands) == true;

    public partial void AddCommandsProjSpecific()
    {
      AddedCommands.Add(new DebugConsole.Command("rad_printmodel", "", Rad_PrintModel_Command));
      AddedCommands.Add(new DebugConsole.Command("rad_save", "", Rad_Save_Command));
      AddedCommands.Add(new DebugConsole.Command("rad_load", "", Rad_Load_Command));
      AddedCommands.Add(new DebugConsole.Command("rad_debugmodel", "", Rad_DebugModel_Command,
        () => new string[][] { Mod.ModelManager.Models.ModelByName.Keys.ToArray() }
      ));
      AddedCommands.Add(new DebugConsole.Command("rad_amount", "", Rad_Amount_Command));
      AddedCommands.Add(new DebugConsole.Command("rad_vanilla", "", Rad_Vanilla_Command,
        () => new string[][] { RadiationParamsAccess.Instance.GetPropNames().Append("reset").ToArray() }
      ));
      AddedCommands.Add(new DebugConsole.Command("campaign_metadata", "", Campaign_Metadata_Command,
        () => new string[][] { CampaignMetadataAccess.Data.Keys.Select(id => id.Value).ToArray() }
      ));
    }


    public static void Rad_Save_Command(string[] args)
    {
      if (args.Length == 0)
      {
        Mod.Logger.Log($"Where?");
        return;
      }

      SimpleResult result = Mod.Config.SaveToModSettings(args[0]);

      if (result.Ok)
      {
        Mod.Logger.Log($"Saved to [{Logger.WrapInColor(Mod.Config.GetPathInModSettings(args[0]), "white")}]");
      }
      else
      {
        Mod.Logger.Log($"Failed: [{Logger.WrapInColor(result.Details, "white")}]");
      }
    }

    public static void Rad_Load_Command(string[] args)
    {
      if (args.Length == 0)
      {
        Mod.Logger.Log($"Which one? {Logger.Wrap.IEnumerable(MainConfig.AvailableConfigs)}");
        return;
      }

      SimpleResult result = Mod.Config.LoadFromModSettings(args[0]);

      if (result.Ok)
      {
        Mod.Logger.Log($"Loaded from [{Mod.Config.GetPathInModSettings(args[0])}]");
      }
      else
      {
        Mod.Logger.Log($"Failed: [{result.Details}]");
      }

    }


    public static void Rad_DebugModel_Command(string[] args)
    {
      if (args.Length == 0)
      {
        Mod.Logger.Log($"Model Debug:");
        Mod.Logger.Log(Logger.Wrap.IDictionary(
          Mod.ModelManager.Models.ModelByName.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Debug
          )
        ));

        return;
      }

      string name = args[0];

      if (Mod.ModelManager.Models.ModelByName.ContainsKey(name))
      {
        Mod.ModelManager.Models.ModelByName[name].Debug = !Mod.ModelManager.Models.ModelByName[name].Debug;
        Mod.Logger.Log($"[{Logger.WrapInColor(name, "white")}] model debug is [{Logger.WrapInColor(Mod.ModelManager.Models.ModelByName[name].Debug, "white")}]");
      }
      else
      {
        Mod.Logger.Log($"No such model [{name}]");
      }
    }
    public static void Rad_PrintModel_Command(string[] args)
    {
      Mod.Logger.Log(CurrentModel);
    }

    public static void Campaign_Metadata_Command(string[] args)
    {
      if (args.Length == 0)
      {
        Mod.Logger.Log(Logger.Wrap.IDictionary(CampaignMetadataAccess.Data));
        return;
      }

      if (args.Length > 1)
      {
        if (!DoIHavePermissions())
        {
          Mod.Logger.Log($"You don't have permissions");
          return;
        }

        CampaignMetadataAccess.Set(args[0], args[1]);

        if (GameMain.IsMultiplayer)
        {
          GameMain.Client.SendConsoleCommand($"campaign_metadata {args[0]} {args[1]}");
        }
      }

      if (args.Length > 0)
      {
        Mod.Logger.Log(CampaignMetadataAccess.Get(args[0]));
      }
    }


    public static void Rad_Vanilla_Command(string[] args)
    {
      if (args.Length == 0)
      {
        Mod.Logger.Log(RadiationParamsAccess.Instance);
        return;
      }

      if (args.Length == 1)
      {
        if (String.Equals(args[0], "reset", StringComparison.OrdinalIgnoreCase))
        {
          RadiationParamsAccess.Instance.Reset();
          Mod.Logger.Log(RadiationParamsAccess.Instance);
          return;
        }

        Mod.Logger.Log($"{args[0]} = {RadiationParamsAccess.Instance.Get(args[0])}");
        return;
      }

      if (args.Length > 1)
      {
        if (!DoIHavePermissions())
        {
          Mod.Logger.Log($"You don't have permissions");
          return;
        }

        RadiationParamsAccess.Instance.Set(args[0], args[1]);

        if (GameMain.IsMultiplayer)
        {
          GameMain.Client.SendConsoleCommand($"rad_vanilla {args[0]} {args[1]}");
        }

        Mod.Logger.Log($"{args[0]} = {RadiationParamsAccess.Instance.Get(args[0])}");
      }
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

        if (float.TryParse(args[0], out float amount))
        {
          GameMain.GameSession.Map.Radiation.Amount = amount;
          Mod.CurrentModel.MetadataSetter?.SetMetadata();
          //TODO should i do full Radiation.OnStep here?

          if (GameMain.IsMultiplayer)
          {
            GameMain.Client.SendConsoleCommand($"rad_amount {args[0]}");
          }
        }


      }

      Mod.Logger.Log($"Rad front: [{GameMain.GameSession.Map.Radiation.Amount}] Current location: [{Level.Loaded.StartLocation.MapPosition.X}{(Level.Loaded.EndLocation is null ? "" : $"-{Level.Loaded.EndLocation.MapPosition.X}")}] Camera irradiation: [{Utils.CameraIrradiation()}] Map width: [{GameMain.GameSession.Map?.Width}]");
    }

  }
}