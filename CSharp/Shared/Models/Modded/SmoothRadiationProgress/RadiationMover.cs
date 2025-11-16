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
  public partial class SmoothRadiationProgressModel
  {
    public class SmoothRadiationMover : IRadiationMover
    {
      public ModelSettings Settings { get; set; }

      public void InitOnFirstRound()
      {
        if (GameMain.GameSession.Map.Radiation?.Enabled == true)
        {
          GameMain.GameSession.Map.Radiation.Amount = Settings.StartingRadiation;
        }
      }

      public void MoveRadiation(Radiation _, float steps)
      {
        if (!_.Enabled) return;
        if (steps <= 0) return;

        float percentageCovered = _.Amount / _.Map.Width;
        float speedMult = Math.Clamp(1 - (1 - Settings.TargetSpeedPercentageAtTheEndOfTheMap) * percentageCovered, 0, 1);

        float increaseAmount = Math.Max(0, Settings.RadiationSpeed * speedMult * steps);

        _.IncreaseRadiation(increaseAmount);
      }

    }
  }
}