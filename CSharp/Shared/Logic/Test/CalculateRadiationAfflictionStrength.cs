using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public class CalculateRadiationAfflictionStrengthTest : UTestPack
  {

  }

  [UTestSubPackOf(typeof(CalculateRadiationAfflictionStrengthTest))]
  public class CalculateRadiationAfflictionStrengthVanillaTest : UTestPack
  {

    public void CreateVanillaTests()
    {
      Tests.Add(new UTest(
        CalculateRadiationAfflictionStrength.DepthInRadiation(new Vector2(150, 0), 200), 50.0f
      ));
    }
  }

}