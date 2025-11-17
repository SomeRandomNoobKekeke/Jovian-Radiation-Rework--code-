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
using Barotrauma.Networking;

namespace JovianRadiationRework
{

  public partial class UpgradeManagerPatch
  {
    public static void PatchSharedUpgradeManager(Harmony harmony)
    {
      harmony.Patch(
        original: typeof(UpgradeManager).GetMethod("TryPurchaseUpgrade", AccessTools.all),
        postfix: new HarmonyMethod(typeof(UpgradeManagerPatch).GetMethod("UpgradeManager_TryPurchaseUpgrade_Postfix"))
      );
    }


    public static void UpgradeManager_TryPurchaseUpgrade_Postfix(UpgradeManager __instance, ref bool __result, UpgradePrefab prefab, UpgradeCategory category, bool force = false, Client? client = null)
    {
      if (prefab.Identifier == RadProtectionUpgradesModel.RadProtectionUpgrades.HullRadProtectionUpgradeID)
      {
        //MB i should notify lifecycle hooks and call HullUpgrades from there
        Mod.CurrentModel.HullUpgrades?.UpdateMainSubProtection();
      }
    }
  }

}