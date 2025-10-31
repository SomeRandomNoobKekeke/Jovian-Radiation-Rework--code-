global using BaroJunk;
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
  public static class Utils
  {

    public static float CurrentLocationRadiationAmount()
    {
      if (
        GameMain.GameSession?.Map?.CurrentLocation == null ||
        GameMain.GameSession.Map.Radiation?.Enabled != true ||
        GameMain.GameSession.Campaign == null
      ) { return 0; }

      return Math.Max(0,
        GameMain.GameSession.Map.Radiation.Amount
        - GameMain.GameSession.Map.CurrentLocation.MapPosition.X
      );
    }

    public static float CameraIrradiation()
    {
      if (
        GameMain.GameSession?.Map?.CurrentLocation == null ||
        GameMain.GameSession.Map.Radiation?.Enabled != true ||
        GameMain.GameSession.Campaign == null
      ) { return 0; }

      return Mod.CurrentModel.WorldPosRadAmountCalculator.CalculateAmount(
        GameMain.GameSession.Map.Radiation,
        new Vector2(
          Screen.Selected.Cam.Position.X,
          Screen.Selected.Cam.Position.Y
        )
      );
    }


  }

}