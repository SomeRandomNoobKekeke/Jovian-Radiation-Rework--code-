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
  public partial class RadProtectionUpgradesModel
  {
    public class RadProtectionUpgrades : IHullUpgrades
    {
      public static Identifier HullRadProtectionUpgradeID = new Identifier("increaseradprotection");

      public ModelSettings Settings { get; set; }
      public RadProtectionUpgradesModel Model { get; set; }


      public float ProtectionFromUpgrades { get; set; }

      public float GetProtectionPerUpgrade() => Settings.HullUpgradeProtection;
      public void UpdateMainSubProtection()
      {
        UpgradePrefab prefab = UpgradePrefab.Find(HullRadProtectionUpgradeID);

        if (prefab is null)
        {
          ProtectionFromUpgrades = 0;
          return;
        }

        int level = GameMain.GameSession?.Campaign?.UpgradeManager?.GetUpgradeLevel(prefab, prefab.UpgradeCategories.First()) ?? 0;
        ProtectionFromUpgrades = Settings.HullUpgradeProtection * level;

        Model.DebugLog($"UpdateMainSubProtection [{ProtectionFromUpgrades}]");
      }

      public float GetProtectionForMainSub() => ProtectionFromUpgrades;
    }
  }
}