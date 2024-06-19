using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

using Barotrauma.Extensions;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static bool Radiation_OnStep_Replace(float steps, Radiation __instance)
    {
      Radiation _ = __instance;

      if (!_.Enabled) { return false; }
      if (steps <= 0) { return false; }

      float increaseAmount = _.Params.RadiationStep * steps;

      if (_.Params.MaxRadiation > 0 && _.Params.MaxRadiation < _.Amount + increaseAmount)
      {
        increaseAmount = _.Params.MaxRadiation - _.Amount;
      }

      _.IncreaseRadiation(increaseAmount);

      int amountOfOutposts = _.Map.Locations.Count(location => location.Type.HasOutpost && !location.IsCriticallyRadiated());

      foreach (Location location in _.Map.Locations.Where(_.Contains))
      {
        if (location.IsGateBetweenBiomes)
        {
          location.Connections.ForEach(c => c.Locked = false);
          continue;
        }

        if (amountOfOutposts <= _.Params.MinimumOutpostAmount) { break; }

        if (_.Map.CurrentLocation is { } currLocation)
        {
          // Don't advance on nearby locations to avoid buggy behavior
          if (currLocation == location || currLocation.Connections.Any(lc => lc.OtherLocation(currLocation) == location)) { continue; }
        }

        bool wasCritical = location.IsCriticallyRadiated();

        location.TurnsInRadiation++;

        if (location.Type.HasOutpost && !wasCritical && location.IsCriticallyRadiated())
        {
          location.ClearMissions();
          amountOfOutposts--;
        }
      }

      return false;
    }

    public static bool Radiation_UpdateRadiation_Replace(float deltaTime, Radiation __instance)
    {
      Radiation _ = __instance;

      if (!(GameMain.GameSession?.IsCurrentLocationRadiated() ?? false)) { return false; }

      if (GameMain.NetworkMember is { IsClient: true }) { return false; }

      if (_.radiationTimer > 0)
      {
        _.radiationTimer -= deltaTime;
        return false;
      }

      if (_.radiationAffliction == null)
      {
        float radiationStrengthChange = AfflictionPrefab.RadiationSickness.Effects.FirstOrDefault()?.StrengthChange ?? 0.0f;
        _.radiationAffliction = new Affliction(
            AfflictionPrefab.RadiationSickness,
            (_.Params.RadiationDamageAmount - radiationStrengthChange) * _.Params.RadiationDamageDelay);
      }

      _.radiationTimer = _.Params.RadiationDamageDelay;

      foreach (Character character in Character.CharacterList)
      {
        if (!character.IsOnPlayerTeam || character.IsDead || character.Removed || !(character.CharacterHealth is { } health)) { continue; }

        float radiationAmount = EntityRadiationAmount(character) * settings.modSettings.RadiationDamage;

        if (character.IsHuskInfected)
        {
          info("it's a husk");
          radiationAmount = Math.Max(0, radiationAmount - settings.modSettings.HuskRadiationResistance * GameMain.GameSession.Map.Radiation.Params.RadiationDamageDelay);
        }
        info(radiationAmount / GameMain.GameSession.Map.Radiation.Params.RadiationDamageDelay);

        if (radiationAmount > 0)
        {
          var limb = character.AnimController.MainLimb;
          AttackResult attackResult = limb.AddDamage(
            limb.SimPosition,
            AfflictionPrefab.RadiationSickness.Instantiate(radiationAmount).ToEnumerable(),
            playSound: false
          );

          // CharacterHealth.ApplyAffliction is simpler but it ignores gear
          character.CharacterHealth.ApplyDamage(limb, attackResult);
        }
      }

      return false;
    }

  }
}