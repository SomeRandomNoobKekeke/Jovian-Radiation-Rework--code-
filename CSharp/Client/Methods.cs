using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
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
          - RelativeDepth * settings.modSettings.WaterRadiationBlockPerMeter
        );

        return amount;
      }

      return 0;
    }
  }
}