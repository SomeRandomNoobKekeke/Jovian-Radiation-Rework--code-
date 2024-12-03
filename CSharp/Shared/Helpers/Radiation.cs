using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

using System.IO;


namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static float EntityRadiationAmount(Entity entity)
    {
      if (entity == null) return 0;
      if (GameMain.GameSession?.Map?.Radiation == null) return 0;

      if (!GameMain.GameSession.Map.Radiation.Enabled) { return 0; }
      if (Level.Loaded is { Type: LevelData.LevelType.LocationConnection, StartLocation: { } startLocation, EndLocation: { } endLocation } level)
      {
        float distance = MathHelper.Clamp((entity.WorldPosition.X - level.StartPosition.X) / (level.EndPosition.X - level.StartPosition.X), 0.0f, 1.0f);

        float RelativeDepth = -(entity.WorldPosition.Y - Math.Max(level.StartPosition.Y, level.EndPosition.Y)) * Physics.DisplayToRealWorldRatio;

        float entityMapX = startLocation.MapPosition.X + (endLocation.MapPosition.X - startLocation.MapPosition.X) * distance;

        return GameMain.GameSession.Map.Radiation.Amount - entityMapX - RelativeDepth * settings.Mod.WaterRadiationBlockPerMeter;
      }
      else
      {
        return CurrentLocationRadiationAmount();
      }

      return 0;
    }

    public static float CurrentLocationRadiationAmount()
    {
      if (
        GameMain.GameSession?.Map?.CurrentLocation == null ||
        GameMain.GameSession.Map.Radiation == null ||
        !GameMain.GameSession.Map.Radiation.Enabled ||
        GameMain.GameSession.Campaign == null
      ) { return 0; }

      return GameMain.GameSession.Map.Radiation.Amount - GameMain.GameSession.Map.CurrentLocation.MapPosition.X;
    }

#if CLIENT
    public static float CameraIrradiation(Camera cam)
    {
      if (GameMain.GameSession?.Map?.Radiation == null || !GameMain.GameSession.Map.Radiation.Enabled) return 0;


      if (Level.Loaded is { Type: LevelData.LevelType.LocationConnection, StartLocation: { } startLocation, EndLocation: { } endLocation } level)
      {
        float distance = MathHelper.Clamp((cam.Position.X - level.StartPosition.X) / (level.EndPosition.X - level.StartPosition.X), 0.0f, 1.0f);

        float RelativeDepth = -(cam.Position.Y - Math.Max(level.StartPosition.Y, level.EndPosition.Y)) * Physics.DisplayToRealWorldRatio;

        float camMapX = startLocation.MapPosition.X + (endLocation.MapPosition.X - startLocation.MapPosition.X) * distance;

        float amount = Math.Max(0,
          GameMain.GameSession.Map.Radiation.Amount
          - camMapX
          - RelativeDepth * settings.Mod.WaterRadiationBlockPerMeter
        );

        return amount;
      } else {
        return  CurrentLocationRadiationAmount();
      }

      return 0;
    }
#endif

  }
}