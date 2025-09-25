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

  public partial class LocationPatch
  {
    public static void PatchSharedLocation(Harmony harmony)
    {
      harmony.Patch(
        original: typeof(Location).GetMethod("IsCriticallyRadiated", AccessTools.all),
        prefix: new HarmonyMethod(typeof(LocationPatch).GetMethod("Location_IsCriticallyRadiated_Replace"))
      );
    }


    public static bool Location_IsCriticallyRadiated_Replace(Location __instance, ref bool __result)
    {
      __result = Mod.CurrentModel.LocationIsCriticallyRadiated.IsIt(__instance);
      return false;
    }


  }

}