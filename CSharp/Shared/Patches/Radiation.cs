using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml.Linq;

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
      if (settings.modSettings.UseVanillaRadiation) return true;

      Radiation _ = __instance;

      if (!_.Enabled) { return false; }
      if (steps <= 0) { return false; }


      float percentageCovered = _.Amount / _.Map.Width;
      float speedMult = 1 - (1 - settings.modSettings.Progress.TargetSpeedPercentageAtTheEndOfTheMap) * percentageCovered;


      info($"map.width {_.Map.Width} Amount {_.Amount} speedMult {speedMult}");

      // deprecated, set it to 0
      float slowDown = _.Amount * settings.modSettings.Progress.RadiationSlowDown;
      info($"Amount: {_.Amount}, slowDown:{slowDown}");

      float increaseAmount = Math.Max(0, (_.Params.RadiationStep * speedMult - slowDown) * steps);

      if (_.Params.MaxRadiation > 0 && _.Params.MaxRadiation < _.Amount + increaseAmount)
      {
        increaseAmount = _.Params.MaxRadiation - _.Amount;
      }


      info($"Radiation.Amount += {increaseAmount}");
      _.IncreaseRadiation(increaseAmount);

      int amountOfOutposts = _.Map.Locations.Count(location => location.Type.HasOutpost && !location.IsCriticallyRadiated());

      foreach (Location location in _.Map.Locations.Where(_.Contains))
      {
        if (location.IsGateBetweenBiomes)
        {
          location.Connections.ForEach(c => c.Locked = false);
        }

        if (amountOfOutposts <= _.Params.MinimumOutpostAmount) { break; }

        if (settings.modSettings.Progress.KeepSurroundingOutpostsAlive && _.Map.CurrentLocation is { } currLocation)
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
      if (settings.modSettings.UseVanillaRadiation) return true;

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
        if (character.Submarine != null)
        {
          radiationAmount *= Math.Clamp(1 - settings.modSettings.FractionOfRadiationBlockedInSub, 0, 1);
        }

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

    public static void Radiation_Constructor_Postfix(Map map, RadiationParams radiationParams, XElement element, Radiation __instance)
    {
      if (element == null)
      {
        __instance.Amount = settings.vanilla.StartingRadiation;
      }
    }

  }
}