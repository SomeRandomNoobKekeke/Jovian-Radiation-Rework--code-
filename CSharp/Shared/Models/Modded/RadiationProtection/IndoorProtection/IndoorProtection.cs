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
  public enum EntityPositionType
  {
    OpenWater, PlayerSub, Beacon, Outpost, EnemySub, Wreck, Ruins, Cave,
  }

  public partial class IndoorProtectionModel
  {
    /// <summary>
    /// Very confusing: Protection = [0: no protection, 1: invulnerable]
    /// ProtectionMult = incoming damage multiplicator = [0 : no damage, 1: full damage]
    /// Damage = rawDamage * ProtectionMult
    /// </summary>
    public class IndoorProtection : IIndoorProtectionCalculator
    {
      public IndoorProtectionModel Model { get; set; }
      public ModelSettings Settings { get; set; }

      public float GetBasicMainSubProtection() => Settings.MainSub;

      public float GetMainsubElectronicsProtectionMult()
        => Math.Clamp(
          1 - (Settings.MainSub + (Mod.CurrentModel.HullUpgrades?.GetProtectionForMainSub() ?? 0)),
          0, 1
        );
      private float openGapFactor(Hull CurrentHull)
      {
        if (CurrentHull is null) return 0; // bruh

        float gapSize = 0;
        foreach (Gap g in CurrentHull.ConnectedGaps)
        {
          if (g.linkedTo.Count == 1) gapSize += g.Open;
        }

        return Math.Clamp(1 - gapSize, 0, 1);
      }

      public bool IsInACave(Entity entity)
      {
        foreach (Level.Cave cave in Level.Loaded.Caves)
        {
          if (cave.Area.Contains(entity.WorldPosition))
          {
            return true;
          }
        }

        return false;
      }

      private EntityPositionType GetEntityPositionType(Entity entity)
      {
        if (entity.Submarine is null)
        {
          return IsInACave(entity) ? EntityPositionType.Cave : EntityPositionType.OpenWater;
        }

        return entity.Submarine.Info.Type switch
        {
          SubmarineType.Player => EntityPositionType.PlayerSub,
          SubmarineType.Outpost => EntityPositionType.Outpost,
          SubmarineType.OutpostModule => EntityPositionType.Outpost,
          SubmarineType.Wreck => EntityPositionType.Wreck,
          SubmarineType.BeaconStation => EntityPositionType.Beacon,
          SubmarineType.EnemySubmarine => EntityPositionType.EnemySub,
          SubmarineType.Ruin => EntityPositionType.Ruins,
          _ => EntityPositionType.OpenWater,
        };
      }



      public float GetHullProtectionMult(Radiation _, Entity entity)
      {
        EntityPositionType position = GetEntityPositionType(entity);

        Hull CurrentHull = entity switch
        {
          Character character => character.CurrentHull,
          Item item => item.CurrentHull,
        };

        float protection = position switch
        {
          EntityPositionType.OpenWater => 0,
          EntityPositionType.Cave => Settings.Cave,
          EntityPositionType.PlayerSub => (Settings.MainSub + (Mod.CurrentModel.HullUpgrades?.GetProtectionForMainSub() ?? 0)) * openGapFactor(CurrentHull),
          EntityPositionType.Beacon => Settings.Beacon * openGapFactor(CurrentHull),
          EntityPositionType.Outpost => Settings.Outpost * openGapFactor(CurrentHull),
          EntityPositionType.EnemySub => Settings.EnemySub * openGapFactor(CurrentHull),
          EntityPositionType.Wreck => Settings.Wreck * openGapFactor(CurrentHull),
          EntityPositionType.Ruins => Settings.Ruins * openGapFactor(CurrentHull),
        };

        return Math.Clamp(1 - protection, 0, 1);
      }
    }
  }
}