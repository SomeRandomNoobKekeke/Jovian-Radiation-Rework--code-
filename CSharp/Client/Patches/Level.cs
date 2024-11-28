using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

using Barotrauma.Networking;
using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Barotrauma.Extensions;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {

    [HarmonyPatch(typeof(Level))]
    public class LevelPatch
    {
      [HarmonyPostfix]
      [HarmonyPatch("DrawBack")]
      public static void Level_DrawBack_Replace(GraphicsDevice graphics, SpriteBatch spriteBatch, Camera cam, Level __instance)
      {
        if (settings.Mod.UseVanillaRadiation) return;
        float time = (float)(Timing.TotalTime / 50.0f);

        float brightness = Math.Clamp(CameraIrradiation(cam) * settings.Mod.RadiationToAmbienceBrightness, 0, settings.Mod.MaxAmbienceBrightness);

        float rad = brightness - PerlinNoise.GetPerlin(time, time * 0.5f) * settings.Mod.MaxAmbienceBrightness;

        GameMain.LightManager.AmbientLight = GameMain.LightManager.AmbientLight.Add(settings.Mod.ActualColor.Multiply(rad));
      }
    }


  }
}