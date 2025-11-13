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

      //WHY is it here? where should it be? it's so cursed
      public float CalculateAmountForCharacter(Radiation _, Character character)
      {
        float amount = CalculateAmount(_, character.WorldPosition);

        if (Mod.CurrentModel.HullProtectionCalculator is not null)
        {
          amount *= Mod.CurrentModel.HullProtectionCalculator.GetHullProtectionMult(_, character);
        }

        if (Mod.CurrentModel.HuskResistanceCalculator is not null)
        {
          amount *= Mod.CurrentModel.HuskResistanceCalculator.GetHuskResistanceMult(_, character);
        }

        return amount;
      }

      public float CalculateAmountForItem(Radiation _, Item item)
      {
        float amount = CalculateAmount(_, item.WorldPosition);

        if (Mod.CurrentModel.HullProtectionCalculator is not null)
        {
          amount *= Mod.CurrentModel.HullProtectionCalculator.GetHullProtectionMult(_, item);
        }

        return amount;
      }

      public float CalculateAmount(Radiation _, Vector2 pos)
      {
        if (!RadiationAccessor.Enabled(_)) return 0;
        if (!Level_Loaded.IsLoaded) return 0;

        if (Level_Loaded is { Type: LevelData.LevelType.LocationConnection })
        {
          float distanceNormalized = (pos.X - Level_Loaded.StartPosition.X) / (Level_Loaded.EndPosition.X - Level_Loaded.StartPosition.X);

          float MapX = Level_Loaded.StartLocation_MapPosition.X + (Level_Loaded.EndLocation_MapPosition.X - Level_Loaded.StartLocation_MapPosition.X) * distanceNormalized;

          float RelativeDepth = (Math.Max(Level_Loaded.StartPosition.Y, Level_Loaded.EndPosition.Y) - pos.Y) * Physics.DisplayToRealWorldRatio;

          return RadiationAccessor.Amount(_)
                 - MapX
                 - RelativeDepth * Settings.WaterRadiationBlockPerMeter;
        }

        if (Level_Loaded is { Type: LevelData.LevelType.Outpost })
        {
          float RelativeDepth = (Level_Loaded.StartPosition.Y - pos.Y) * Physics.DisplayToRealWorldRatio;

          return RadiationAccessor.Amount(_)
                 - Level_Loaded.StartLocation_MapPosition.X
                 - RelativeDepth * Settings.WaterRadiationBlockPerMeter;
        }


        return 0;
      }


    }
  }
}