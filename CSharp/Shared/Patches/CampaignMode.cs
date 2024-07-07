using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static bool CampaignMode_HandleSaveAndQuit_Prefix(CampaignMode __instance)
    {
      log("lol you quited!");
      return true;
    }
  }
}