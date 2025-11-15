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

  public partial class CustomRadiationUpdaterModel
  {
    public class CustomRadiationUpdater : IRadiationUpdater
    {
      public ModelSettings Settings { get; set; }

      public float GetUpdateInterval() => Settings.CharacterDamageInterval;

      private float updateTimer;
      private bool ShouldDamage(Radiation _, float deltaTime)
      {
        if (!(GameMain.GameSession?.IsCurrentLocationRadiated() ?? false)) { return false; }

        if (GameMain.NetworkMember is { IsClient: true }) { return false; }

        if (updateTimer > 0)
        {
          updateTimer -= deltaTime;
          return false;
        }

        updateTimer = Settings.CharacterDamageInterval;
        return true;
      }

      private void DamageCharacters(Radiation _, float deltaTime)
      {
        if (!ShouldDamage(_, deltaTime)) return;

        foreach (Character character in Character.CharacterList)
        {
          if (character.IsDead || character.Removed || !(character.CharacterHealth is { } health)) { continue; }

          float radAmount = Math.Max(0,
            Mod.CurrentModel.WorldPosRadAmountCalculator.CalculateAmountForCharacter(
              _, character
            )
          );

          // THINK mb allow custom afflictions to heal something
          if (radAmount == 0) continue;

          if (character.IsHuman)
          {
            Mod.CurrentModel.HumanDamager.DamageHuman(character, radAmount, _);
          }
          else
          {
            Mod.CurrentModel.MonsterDamager.DamageMonster(character, radAmount, _);
          }
        }
      }

      public void UpdateRadiation(Radiation _, float deltaTime)
      {
        Mod.CurrentModel.ElectronicsDamager?.DamageElectronics(_, deltaTime);
        DamageCharacters(_, deltaTime);
      }
    }
  }
}