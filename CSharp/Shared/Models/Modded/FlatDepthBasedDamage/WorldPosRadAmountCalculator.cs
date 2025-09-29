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
  public partial class FlatDepthBasedDamageModel
  {
    public class FlatDepthBasedPosRadAmountCalculator : IWorldPosRadAmountCalculator
    {
      public ModelSettings Settings { get; set; }
      public ILevel Level_Loaded { get; set; }
      public IRadiationAccessor RadiationAccessor { get; set; }

      public float CalculateAmount(Radiation _, Vector2 pos)
      {
        if (!RadiationAccessor.Enabled(_)) return 0;
        if (!Level_Loaded.IsLoaded) return 0;

        if (Level_Loaded is { Type: LevelData.LevelType.LocationConnection })
        {
          float distanceNormalized = (pos.X - Level_Loaded.StartPosition.X) / (Level_Loaded.EndPosition.X - Level_Loaded.StartPosition.X);

          float MapX = Level_Loaded.StartLocation_MapPosition.X + (Level_Loaded.EndLocation_MapPosition.X - Level_Loaded.StartLocation_MapPosition.X) * distanceNormalized;

          float RelativeDepth = (Math.Max(Level_Loaded.StartPosition.Y, Level_Loaded.EndPosition.Y) - pos.Y) * Physics.DisplayToRealWorldRatio;

          return Math.Max(0,
            RadiationAccessor.Amount(_)
            - MapX
            - RelativeDepth * Settings.WaterRadiationBlockPerMeter
          );
        }

        if (Level_Loaded is { Type: LevelData.LevelType.Outpost })
        {
          float RelativeDepth = (Level_Loaded.StartPosition.Y - pos.Y) * Physics.DisplayToRealWorldRatio;

          return Math.Max(0,
            RadiationAccessor.Amount(_)
            - Level_Loaded.StartLocation_MapPosition.X
            - RelativeDepth * Settings.WaterRadiationBlockPerMeter
          );
        }


        return 0;
      }


    }
  }
}