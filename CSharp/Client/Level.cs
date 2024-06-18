using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Barotrauma.Extensions;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static void Level_DrawBack_Postfix(GraphicsDevice graphics, SpriteBatch spriteBatch, Camera cam, Level __instance)
    {
      float time = (float)(Timing.TotalTime / 100.0f);

      float rad = Math.Clamp(CameraIrradiation(cam) * settings.RadiationToColor - PerlinNoise.GetPerlin(time, time * 0.5f) * 0.3f, 0, 0.38f);
      GameMain.LightManager.AmbientLight = GameMain.LightManager.AmbientLight.Add(new Color(0, rad, rad));
    }
  }
}