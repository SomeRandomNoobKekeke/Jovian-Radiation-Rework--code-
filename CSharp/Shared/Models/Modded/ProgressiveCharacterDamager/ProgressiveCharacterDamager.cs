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
  public partial class ProgressiveCharacterDamagerModel
  {
    public class ProgressiveCharacterDamager : ICharacterDamager
    {
      public ProgressiveCharacterDamagerModel Model { get; set; }

      public void DamageCharacter(Character character, float radAmount, Radiation _)
      {
        float dps = radAmount * Model.Settings.RadAmountToDPS;
        float damage = dps * Model.Settings.DamageInterval;

        if (damage > 0)
        {
          var limb = character.AnimController.MainLimb;

          AttackResult attackResult = limb.AddDamage(
            limb.SimPosition,
            Model.Settings.Affliction.AfflictionPrefab.Instantiate(damage).ToEnumerable(),
            playSound: false
          );

          // CharacterHealth.ApplyAffliction is simpler but it ignores gear
          character.CharacterHealth.ApplyDamage(limb, attackResult);
        }
      }
    }
  }
}