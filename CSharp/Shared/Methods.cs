using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;



namespace BetterRadiation
{
  public partial class Mod : IAssemblyPlugin
  {
    //character.CharacterHealth.ApplyAffliction(limb, burnPrefab.Instantiate(50))

    public float HowIrradiatedIsCurrentLocation()
    {
      if (
        GameMain.GameSession?.Map?.CurrentLocation == null ||
        GameMain.GameSession.Map.Radiation == null ||
        GameMain.GameSession.Map.Radiation.Enabled ||
        GameMain.GameSession.Campaign == null
      ) { return 0; }

      return GameMain.GameSession.Map.CurrentLocation.MapPosition.X - GameMain.GameSession.Map.Radiation.Amount;
    }
  }
}