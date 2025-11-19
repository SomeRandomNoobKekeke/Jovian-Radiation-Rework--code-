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
  public partial class CustomCharacterDamagerModel
  {
    public class CustomHumanDamager : IHumanDamager
    {
      public ModelSettings Settings { get; set; }
      public CustomCharacterDamagerModel Model { get; set; }

      public float RadAmountToRadDps(float amount)
        => amount * Settings.RadAmountToDPS;

      public void DamageHuman(Character character, float radAmount, Radiation _)
      {
        if (Mod.CurrentModel.HuskResistanceCalculator is not null)
        {
          radAmount *= Mod.CurrentModel.HuskResistanceCalculator.GetHuskResistanceMult(_, character);
        }

        float rawdps = radAmount * Settings.RadAmountToDPS;
        float dps = rawdps - Settings.ExtraHumanHealing;
        float damage = Math.Max(0, dps * Mod.CurrentModel.RadiationUpdater.GetUpdateInterval());

        Model.DebugLog($"Damaging [{character?.Info?.DisplayName}] with [{damage}] (dps:[{rawdps}] - healing:[{Settings.ExtraHumanHealing}]) [{Settings.Affliction.AfflictionPrefab}]");

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