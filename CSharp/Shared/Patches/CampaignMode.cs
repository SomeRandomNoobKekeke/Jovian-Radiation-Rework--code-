using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {

    [HarmonyPatch(typeof(CampaignMode))]
    public class CampaignModePatch
    {
      [HarmonyPrefix]
      [HarmonyPatch("HandleSaveAndQuit")]
      public static bool CampaignMode_HandleSaveAndQuit_Prefix(CampaignMode __instance)
      {
        return true;
      }
    }


  }
}