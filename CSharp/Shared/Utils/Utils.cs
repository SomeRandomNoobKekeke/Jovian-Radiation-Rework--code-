global using BaroJunk;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public static class Utils
  {


    public static float AverageLocationRadiationAmount(ILevel Level_Loaded, IRadiationAccessor RadiationAccessor)
    {
      if (GameMain.GameSession?.Map?.Radiation?.Enabled != true) return 0;


      if (!Level_Loaded.IsLoaded) return 0;

      if (Level_Loaded.Type == LevelData.LevelType.LocationConnection)
      {
        return Math.Max(0,
          RadiationAccessor.Amount(GameMain.GameSession.Map.Radiation)
          - (Level_Loaded.EndLocation_MapPosition.X - Level_Loaded.StartLocation_MapPosition.X) / 2.0f
        );
      }

      if (Level_Loaded.Type == LevelData.LevelType.Outpost)
      {
        return Math.Max(0,
          RadiationAccessor.Amount(GameMain.GameSession.Map.Radiation)
          - Level_Loaded.StartLocation_MapPosition.X
        );
      }

      return 0;
    }
    public static float CurrentLocationRadiationAmount()
    {
      if (
        GameMain.GameSession?.Map?.CurrentLocation == null ||
        GameMain.GameSession.Map.Radiation?.Enabled != true ||
        GameMain.GameSession.Campaign == null
      ) { return 0; }

      return Math.Max(0,
        GameMain.GameSession.Map.Radiation.Amount
        - GameMain.GameSession.Map.CurrentLocation.MapPosition.X
      );
    }


    public static float CameraIrradiation()
    {
      if (
        GameMain.GameSession?.Map?.CurrentLocation == null ||
        GameMain.GameSession.Map.Radiation?.Enabled != true ||
        GameMain.GameSession.Campaign == null
      ) { return 0; }

      return Mod.CurrentModel.WorldPosRadAmountCalculator.CalculateAmount(
        GameMain.GameSession.Map.Radiation,
        new Vector2(
          Screen.Selected.Cam.Position.X,
          Screen.Selected.Cam.Position.Y
        )
      );
    }


  }

}