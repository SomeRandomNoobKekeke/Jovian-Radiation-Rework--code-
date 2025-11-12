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
  public partial class HullBlocksRadiationModel
  {
    public class HullBlocksRadiation : IHullProtectionCalculator
    {
      public ModelSettings Settings { get; set; }
      public float GetHullProtectionMult(Radiation _, Entity entity)
      {
        float calculateProtectionForHull(Hull CurrentHull)
        {
          if (CurrentHull is null) return 1;

          float gapSize = 0;
          foreach (Gap g in CurrentHull.ConnectedGaps)
          {
            if (g.linkedTo.Count == 1) gapSize += g.Open;
          }

          gapSize = Math.Clamp(gapSize, 0, 1);

          return Math.Clamp(1 - (1 - gapSize) * Settings.FractionOfRadiationBlockedInSub, 0, 1);
        }

        float mult = 1.0f;

        if (entity.Submarine != null)
        {
          if (entity is Character character)
          {
            mult = calculateProtectionForHull(character.CurrentHull);
          }

          if (entity is Item item)
          {
            mult = calculateProtectionForHull(item.CurrentHull);
          }
        }

        return mult;
      }
    }
  }
}