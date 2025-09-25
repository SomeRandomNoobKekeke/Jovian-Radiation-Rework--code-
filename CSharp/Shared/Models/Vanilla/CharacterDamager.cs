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
    public class VanillaCharacterDamager : ICharacterDamager
    {
      public void DamageCharacter(Character character, float radAmount, Radiation _)
      {
        if (radAmount > 0)
        {
          AfflictionPrefab afflictionPrefab;
          // Get the related affliction (if necessary, fall back to the traditional radiation sickness for slightly better backwards compatibility)
          afflictionPrefab = AfflictionPrefab.JovianRadiation ?? AfflictionPrefab.RadiationSickness;
          float currentAfflictionStrength = character.CharacterHealth.GetAfflictionStrengthByIdentifier(afflictionPrefab.Identifier);

          // Get Jovian radiation strength, and cancel out the affliction's strength change (meant for decaying it)
          // (for simplicity, let's assume each Effect of the Affliction has the same strengthchange)
          float addedStrength = _.Params.RadiationDamageAmount - afflictionPrefab.Effects.FirstOrDefault()?.StrengthChange ?? 0.0f;

          // Damage is applied periodically, so we must apply the total damage for the full period at once (after deducting strengthchange)
          addedStrength *= _.Params.RadiationDamageDelay;

          // The JovianRadiation affliction has brackets of 25 strength determined by the multiplier (1x = 0-25, 2x = 25-50 etc.)
          int multiplier = (int)Math.Ceiling(radAmount / _.Params.RadiationEffectMultipliedPerPixelDistance);
          float growthPotentialInBracket = (multiplier * 25) - currentAfflictionStrength;
          if (growthPotentialInBracket > 0)
          {
            addedStrength = Math.Min(addedStrength, growthPotentialInBracket);
            character.CharacterHealth.ApplyAffliction(
                character.AnimController?.MainLimb,
                afflictionPrefab.Instantiate(addedStrength));
          }
        }
      }
    }
  }
}