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
  public class VanillaRadiationUpdater : IRadiationUpdater
  {
    public void UpdateRadiation(Radiation __instance, float deltaTime)
    {
      if (!(GameMain.GameSession?.IsCurrentLocationRadiated() ?? false)) { return; }

      if (GameMain.NetworkMember is { IsClient: true }) { return; }

      if (_.radiationTimer > 0)
      {
        _.radiationTimer -= deltaTime;
        return;
      }

      _.radiationTimer = _.Params.RadiationDamageDelay;

      foreach (Character character in Character.CharacterList)
      {
        if (character.IsDead || character.Removed || !(character.CharacterHealth is { } health)) { continue; }

        float depthInRadiation = Mod.LogicContainer.RadAmountCalculator.CalculateAmount(__instance, character);
        Mod.LogicContainer.CharacterDamager.DamageCharacter(character, depthInRadiation, _);
      }
    }

  }



}