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

  public partial class RadiationPatch
  {

    public static void PatchSharedRadiation(Harmony harmony)
    {
      harmony.Patch(
        original: typeof(Radiation).GetMethod("OnStep", AccessTools.all),
        prefix: new HarmonyMethod(typeof(RadiationPatch).GetMethod("Radiation_OnStep_Replace"))
      );

      harmony.Patch(
        original: typeof(Radiation).GetMethod("UpdateRadiation", AccessTools.all),
        prefix: new HarmonyMethod(typeof(RadiationPatch).GetMethod("Radiation_UpdateRadiation_Replace"))
      );
    }

    // https://github.com/evilfactory/LuaCsForBarotrauma/blob/master/Barotrauma/BarotraumaShared/SharedSource/Map/Map/Radiation.cs#L47
    public static bool Radiation_OnStep_Replace(Radiation __instance, float steps = 1)
    {
      Radiation _ = __instance;

      Mod.CurrentModel.RadiationMover.MoveRadiation(_, steps);
      Mod.CurrentModel.LocationTransformer.TransformLocations(_);
      Mod.CurrentModel.MetadataSetter?.SetMetadata();

      return false;
    }


    // https://github.com/evilfactory/LuaCsForBarotrauma/blob/master/Barotrauma/BarotraumaShared/SharedSource/Map/Map/Radiation.cs#L97
    public static bool Radiation_UpdateRadiation_Replace(Radiation __instance, float deltaTime)
    {
      Radiation _ = __instance;

      Mod.CurrentModel.RadiationUpdater.UpdateRadiation(_, deltaTime);

      return false;
    }


  }

}