using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public class UpgradeStorePatch
  {
    public static void PatchClientUpgradeStore(Harmony harmony)
    {
      harmony.Patch(
        original: typeof(UpgradeStore).GetMethod("UpdateUpgradePercentageText", AccessTools.all),
        prefix: new HarmonyMethod(typeof(UpgradeStorePatch).GetMethod("UpgradeStore_UpdateUpgradePercentageText_Replace"))
      );
    }

    public static void UpgradeStore_UpdateUpgradePercentageText_Replace(ref bool __runOriginal, GUITextBlock text, UpgradePrefab upgradePrefab, int currentLevel)
    {
      if (upgradePrefab.Identifier.Value == "increaseradprotection")
      {
        __runOriginal = false;

        int maxLevel = upgradePrefab.GetMaxLevelForCurrentSub();
        float nextIncrease =
          Mod.CurrentModel.HullProtectionCalculator.GetBasicMainSubProtection() +
          (Mod.CurrentModel.HullUpgrades?.GetProtectionPerUpgrade() ?? 0) *
          Math.Min(currentLevel + 1, maxLevel);

        if (nextIncrease != 0f)
        {
          text.Text = $"{Math.Round(nextIncrease * 100.0f, 1)} %";
          if (currentLevel == maxLevel)
          {
            text.TextColor = Color.Gray;
          }
        }
      }
    }
  }
}