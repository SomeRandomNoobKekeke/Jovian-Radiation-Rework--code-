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
  public partial class CustomCharacterDamager
  {
    public class CustomHumanDamager : IHumanDamager
    {
      public ModelSettings Settings { get; set; }
      public CustomCharacterDamager Model { get; set; }

      public float RadAmountToRadDps(float amount)
        => amount * Settings.RadAmountToDPS;

      public void DamageHuman(Character character, float radAmount, Radiation _)
      {
        float dps = radAmount * Settings.RadAmountToDPS;
        float damage = dps * Math.Max(0, Mod.CurrentModel.RadiationUpdater.GetUpdateInterval());

        Model.DebugLog($"Damaging [{character?.Info?.DisplayName}] with [{damage}] [{Settings.Affliction.AfflictionPrefab}]");

        var limb = character.AnimController.MainLimb;

        AttackResult attackResult = limb.AddDamage(
          limb.SimPosition,
          Settings.Affliction.AfflictionPrefab.Instantiate(damage).ToEnumerable(),
          playSound: false
        );

        // CharacterHealth.ApplyAffliction is simpler but it ignores gear
        character.CharacterHealth.ApplyDamage(limb, attackResult);
      }
    }
  }
}