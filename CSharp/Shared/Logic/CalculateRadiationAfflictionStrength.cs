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
  public static class CalculateRadiationAfflictionStrength
  {
    public static float DepthInRadiation(Vector2 pos, float Amount)
    {
      return Amount - pos.X;
    }

    public static float DepthInRadiation(
      float EntityWorldPositionX,
      bool Enabled,
      float Amount
    )
    {
      if (!Enabled) { return 0; }
      if (Level.Loaded is { Type: LevelData.LevelType.LocationConnection, StartLocation: { } startLocation, EndLocation: { } endLocation } level)
      {
        // Approximate how far between the level start and end points the entity is on the map
        float distanceNormalized = MathHelper.Clamp((EntityWorldPositionX - level.StartPosition.X) / (level.EndPosition.X - level.StartPosition.X), 0.0f, 1.0f);
        var (startX, startY) = startLocation.MapPosition;
        var (endX, endY) = endLocation.MapPosition;
        Vector2 mapPos = new Vector2(startX, startY) + (new Vector2(endX - startX, endY - startY) * distanceNormalized);

        return DepthInRadiation(mapPos, Amount);
      }

      return 0;
    }

    public static float Vanilla(
      float CharacterWorldPositionX,
      float CharacterCurrentAfflictionStrength,
      AfflictionPrefab afflictionPrefab,
      float RadiationDamageAmount,
      float RadiationDamageDelay,
      float RadiationEffectMultipliedPerPixelDistance,
      bool Enabled,
      float Amount
    )
    {
      float depthInRadiation = DepthInRadiation(CharacterWorldPositionX, Enabled, Amount);
      if (depthInRadiation > 0)
      {
        // Get Jovian radiation strength, and cancel out the affliction's strength change (meant for decaying it)
        // (for simplicity, let's assume each Effect of the Affliction has the same strengthchange)
        float addedStrength = RadiationDamageAmount - afflictionPrefab.Effects.FirstOrDefault()?.StrengthChange ?? 0.0f;

        // Damage is applied periodically, so we must apply the total damage for the full period at once (after deducting strengthchange)
        addedStrength *= RadiationDamageDelay;

        // The JovianRadiation affliction has brackets of 25 strength determined by the multiplier (1x = 0-25, 2x = 25-50 etc.)
        int multiplier = (int)Math.Ceiling(depthInRadiation / RadiationEffectMultipliedPerPixelDistance);
        float growthPotentialInBracket = (multiplier * 25) - CharacterCurrentAfflictionStrength;

        addedStrength = Math.Min(addedStrength, growthPotentialInBracket);

        return addedStrength;
      }

      return 0;
    }
  }

}