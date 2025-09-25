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
    public class VanillaRadiationUpdater : IRadiationUpdater
    {
      public void UpdateRadiation(Radiation _, float deltaTime)
      {
        if (!Mod.CurrentModel.RadUpdateCondition.ShouldUpdate(_, deltaTime)) return;

        foreach (Character character in Character.CharacterList)
        {
          if (character.IsDead || character.Removed || !(character.CharacterHealth is { } health)) { continue; }

          float radAmount = Mod.CurrentModel.EntityRadAmountCalculator.CalculateAmount(_, character);
          Mod.CurrentModel.CharacterDamager.DamageCharacter(character, radAmount, _);
        }
      }

    }
  }
}