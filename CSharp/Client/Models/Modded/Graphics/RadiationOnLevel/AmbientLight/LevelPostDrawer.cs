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
using Microsoft.Xna.Framework.Graphics;

namespace JovianRadiationRework
{
  public partial class AmbientLightModel : RadiationModel
  {
    public class AmbientLightLevelPostDrawer : ILevelPostDrawer
    {
      public ModelSettings Settings { get; set; }

      public void Draw(Level _, GraphicsDevice graphics, SpriteBatch spriteBatch, Camera cam)
      {
        Radiation radiation = (GameMain.GameSession?.GameMode as CampaignMode)?.Map?.Radiation;
        if (radiation is null) return;

        float brightness = Math.Clamp(Mod.CurrentModel.WorldPosRadAmountCalculator.CalculateAmount(
            radiation,
            new Vector2(
              cam.Position.X,
              cam.Position.Y
            )
          ) * Settings.RadiationToAmbienceBrightness,
          0, Settings.MaxAmbienceBrightness
        );

        float time = (float)(Timing.TotalTime * Settings.PerlinNoiseFrequency);

        // PerlinNoise.GetPerlin is [0..1]
        float rad = brightness - PerlinNoise.GetPerlin(time, time * 0.5f) * Settings.AmbienceNoiseAmplitude;

        GameMain.LightManager.AmbientLight = GameMain.LightManager.AmbientLight.Add(Settings.ActualColor.Multiply(rad));
      }
    }
  }

}