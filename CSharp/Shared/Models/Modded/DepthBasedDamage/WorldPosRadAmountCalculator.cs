using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

using Barotrauma.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Voronoi2;


namespace JovianRadiationRework
{
  public partial class DepthBasedDamageModel
  {
    public class DepthBasedWorldPosRadAmountCalculator : IWorldPosRadAmountCalculator
    {
      public DepthBasedDamageModel Model { get; set; }

      public float CalculateAmount(Radiation _, Vector2 pos)
      {
        if (!_.Enabled) { return 0; }

        if (Level.Loaded is { Type: LevelData.LevelType.LocationConnection, StartLocation: { } startLocation, EndLocation: { } endLocation } level)
        {
          float distanceNormalized = MathHelper.Clamp((pos.X - level.StartPosition.X) / (level.EndPosition.X - level.StartPosition.X), 0.0f, 1.0f);

          float MapX = startLocation.MapPosition.X + (endLocation.MapPosition.X - startLocation.MapPosition.X) * distanceNormalized;

          float RelativeDepth = (level.StartPosition.Y - pos.Y) * Physics.DisplayToRealWorldRatio;

          return Math.Max(0,
            GameMain.GameSession.Map.Radiation.Amount
            - MapX
            - RelativeDepth * Model.Settings.WaterRadiationBlockPerMeter
          );
        }

        if (Level.Loaded is { Type: LevelData.LevelType.Outpost })
        {
          float RelativeDepth = (Level.Loaded.StartPosition.Y - pos.Y) * Physics.DisplayToRealWorldRatio;

          return Math.Max(0,
            GameMain.GameSession.Map.Radiation.Amount
            - GameMain.GameSession.Map.CurrentLocation.MapPosition.X
            - RelativeDepth * Model.Settings.WaterRadiationBlockPerMeter
          );
        }


        return 0;
      }


    }
  }
}