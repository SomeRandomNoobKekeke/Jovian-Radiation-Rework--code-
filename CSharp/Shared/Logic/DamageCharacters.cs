using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

namespace JovianRadiationRework
{
  public static class DamageCharacters
  {
    public static void Vanilla(Radiation _, Character character)
    {
      if (character.IsDead || character.Removed || !(character.CharacterHealth is { } health)) return;

      AfflictionPrefab afflictionPrefab;
      // Get the related affliction (if necessary, fall back to the traditional radiation sickness for slightly better backwards compatibility)
      afflictionPrefab = AfflictionPrefab.JovianRadiation ?? AfflictionPrefab.RadiationSickness;

      float addedStrength = CalculateRadiationAfflictionStrength.Vanilla(
        character.WorldPosition.X,
        character.CharacterHealth.GetAfflictionStrengthByIdentifier(afflictionPrefab.Identifier),
        afflictionPrefab,
        _.Params.RadiationDamageAmount,
        _.Params.RadiationDamageDelay,
        _.Params.RadiationEffectMultipliedPerPixelDistance,
        _.Enabled,
        _.Amount
      );

      if (addedStrength > 0)
      {
        character.CharacterHealth.ApplyAffliction(
          character.AnimController?.MainLimb,
          afflictionPrefab.Instantiate(addedStrength)
        );
      }
    }
  }

}