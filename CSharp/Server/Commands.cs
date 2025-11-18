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
    public partial void AddCommandsProjSpecific()
    {
      CommandRelay.IsolatedCommands.Add("rad_load", Rad_Load_Command);
      CommandRelay.IsolatedCommands.Add("rad_debugmodel", Rad_DebugModel_Command);
      CommandRelay.IsolatedCommands.Add("rad_amount", Rad_Amount_Command);
    }

    public static void Rad_Load_Command(string[] args)
    {
      if (args.Length == 0)
      {
        Mod.Logger.Log($"Which one? {Logger.Wrap.IEnumerable(MainConfig.AvailableConfigs)}");
        return;
      }

      SimpleResult result = Mod.Config.LoadPreset(args[0]);

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


    public static void Rad_Amount_Command(string[] args)
    {
      if (GameMain.GameSession?.Map?.Radiation is null) return;

      if (args.Length > 0)
      {
        if (float.TryParse(args[0], out float amount))
        {
          GameMain.GameSession.Map.Radiation.Amount = amount;
          Mod.CurrentModel.MetadataSetter?.SetMetadata();
          //TODO should i do full Radiation.OnStep here?
        }
      }

      Mod.Logger.Log($"Rad front: [{GameMain.GameSession.Map.Radiation.Amount}] Current location: [{Level.Loaded.StartLocation.MapPosition.X}{(Level.Loaded.EndLocation is null ? "" : $"-{Level.Loaded.EndLocation.MapPosition.X}")}] Camera irradiation: [{Utils.CameraIrradiation()}] Map width: [{GameMain.GameSession.Map?.Width}]");
    }
  }
}