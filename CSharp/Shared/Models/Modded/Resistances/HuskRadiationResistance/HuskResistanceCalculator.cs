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
  public partial class HuskRadiationResistanceModel
  {
    public class CustomHuskResistanceCalculator : IHuskResistanceCalculator
    {
      public HuskRadiationResistanceModel Model { get; set; }
      public ModelSettings Settings { get; set; }

      public float GetHuskInfectionStrength(Character character)
      {
        return character.CharacterHealth.GetAfflictionStrengthByIdentifier(
          AfflictionPrefab.HuskInfection.Identifier
        );
      }
      public float GetHuskResistanceMult(Radiation _, Character character)
      {
        Model.DebugLog($"{character.Info?.DisplayName} {GetHuskInfectionStrength(character)} isHusk:[{GetHuskInfectionStrength(character) > Settings.HuskInfectionThreshold}]");

        return GetHuskInfectionStrength(character) > Settings.HuskInfectionThreshold ?
               Settings.HuskRadiationResistanceMult : 1.0f;
      }
    }
  }
}