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

      public void DamageHumans(Radiation _, float deltaTime)
      {
        if (!ShouldDamage(_, deltaTime)) return;

        foreach (Character character in Character.CharacterList)
        {
          if (character.IsDead || character.Removed || !character.IsHuman || !(character.CharacterHealth is { } health)) { continue; }

          float radAmount = Math.Max(0,
            Mod.CurrentModel.WorldPosRadAmountCalculator.CalculateAmountForCharacter(
              _, character
            )
          );

          // THINK mb allow custom afflictions to heal something
          if (radAmount == 0) continue;

          DamageHuman(character, radAmount, _);
        }
      }

      public bool ShouldDamage(Radiation _, float deltaTime)
      {
        if (!(GameMain.GameSession?.IsCurrentLocationRadiated() ?? false)) { return false; }

        if (GameMain.NetworkMember is { IsClient: true }) { return false; }

        if (_.radiationTimer > 0)
        {
          _.radiationTimer -= deltaTime;
          return false;
        }

        _.radiationTimer = Settings.DamageInterval;
        return true;
      }

      public void DamageHuman(Character character, float radAmount, Radiation _)
      {
        float dps = radAmount * Settings.RadAmountToDPS;
        float damage = dps * Math.Max(0, Settings.DamageInterval);

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