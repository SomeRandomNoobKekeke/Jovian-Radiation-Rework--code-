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
  public partial class VanillaRadiationModel
  {
    public class VanillaWorldPosRadAmountCalculator : IWorldPosRadAmountCalculator
    {
      public float RadAmountToRadDps(float amount)
        => Mod.CurrentModel.HumanDamager.RadAmountToRadDps(amount);

      public float CalculateAmount(Radiation _, Vector2 pos)
      {
        if (!_.Enabled) { return 0; }
        if (Level.Loaded is { Type: LevelData.LevelType.LocationConnection, StartLocation: { } startLocation, EndLocation: { } endLocation } level)
        {
          // Approximate how far between the level start and end points the entity is on the map
          float distanceNormalized = MathHelper.Clamp((pos.X - level.StartPosition.X) / (level.EndPosition.X - level.StartPosition.X), 0.0f, 1.0f);
          var (startX, startY) = startLocation.MapPosition;
          var (endX, endY) = endLocation.MapPosition;
          Vector2 mapPos = new Vector2(startX, startY) + (new Vector2(endX - startX, endY - startY) * distanceNormalized);

          return _.DepthInRadiation(mapPos);
        }

        return 0;
      }


    }
  }
}