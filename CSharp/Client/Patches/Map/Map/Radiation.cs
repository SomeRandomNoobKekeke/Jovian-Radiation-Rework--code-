using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

using Barotrauma.Networking;
using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace JovianRadiationRework
{

  public partial class RadiationPatch
  {

    public static void PatchClientRadiation(Harmony harmony)
    {
      harmony.Patch(
        original: typeof(Radiation).GetMethod("Draw", AccessTools.all),
        prefix: new HarmonyMethod(typeof(RadiationPatch).GetMethod("Radiation_Draw_Replace"))
      );

      harmony.Patch(
        original: typeof(Radiation).GetMethod("DrawFront", AccessTools.all),
        prefix: new HarmonyMethod(typeof(RadiationPatch).GetMethod("Radiation_DrawFront_Replace"))
      );

      // harmony.Patch(
      //   original: typeof(Radiation).GetMethod("MapUpdate", AccessTools.all),
      //   prefix: new HarmonyMethod(typeof(RadiationPatch).GetMethod("Radiation_MapUpdate_Replace"))
      // );
    }

    //https://github.com/FakeFishGames/Barotrauma/blob/51db93fabcb4751b11b79b8f55e6ef3c5f9afec9/Barotrauma/BarotraumaClient/ClientSource/Map/Map/Radiation.cs#L17
    public static void Radiation_Draw_Replace(Radiation __instance, ref bool __runOriginal, SpriteBatch spriteBatch, Rectangle container, float zoom)
    {
      __runOriginal = false;
      Radiation _ = __instance;


      if (!_.Enabled) { return; }

      Mod.CurrentModel.MapRadiationDrawer.Draw(_, spriteBatch, container, zoom);
    }

    //https://github.com/FakeFishGames/Barotrauma/blob/51db93fabcb4751b11b79b8f55e6ef3c5f9afec9/Barotrauma/BarotraumaClient/ClientSource/Map/Map/Radiation.cs#L17
    public static void Radiation_DrawFront_Replace(Radiation __instance, ref bool __runOriginal, SpriteBatch spriteBatch)
    {
      __runOriginal = false;
      Radiation _ = __instance;

      if (_.radiationMultiplier is int multiplier)
      {
        var tooltip = Mod.CurrentModel.MapRadiationTooltip.GetText(_);
        GUIComponent.DrawToolTip(spriteBatch, tooltip, PlayerInput.MousePosition + new Vector2(18 * GUI.Scale));
      }
    }

    //https://github.com/FakeFishGames/Barotrauma/blob/51db93fabcb4751b11b79b8f55e6ef3c5f9afec9/Barotrauma/BarotraumaClient/ClientSource/Map/Map/Radiation.cs#L17
    public static void Radiation_MapUpdate_Replace(Radiation __instance, ref bool __runOriginal, float deltaTime)
    {
      __runOriginal = false;
      Radiation _ = __instance;

      float spriteStep = _.Params.BorderAnimationSpeed * deltaTime;
      Radiation.spriteIndex = (Radiation.spriteIndex + spriteStep) % _.maxFrames;

      if (_.increasedAmount > 0)
      {
        _.increasedAmount -= (_.lastIncrease / _.Params.AnimationSpeed) * deltaTime;
      }
    }



  }

}