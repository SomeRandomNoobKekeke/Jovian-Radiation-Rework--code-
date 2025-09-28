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


namespace JovianRadiationRework
{

  public partial class CampaignModePatch
  {
    public static void PatchSharedCampaignMode(Harmony harmony)
    {
      harmony.Patch(
        original: typeof(CampaignMode).GetMethod("HandleSaveAndQuit", AccessTools.all),
        prefix: new HarmonyMethod(typeof(CampaignModePatch).GetMethod("CampaignMode_HandleSaveAndQuit_Prefix"))
      );
    }


    public static void CampaignMode_HandleSaveAndQuit_Prefix(CampaignMode __instance)
    {
      Mod.CurrentModel.SaveAndQuitHandler?.HandleSaveAndQuit(__instance);
    }


  }

}