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
    public class LevelPostDrawerAspect : ILevelPostDrawer
    {
      public AmbientLightModel Model { get; set; }
      public void AcceptModel(RadiationModel model) => this.Model = model as AmbientLightModel;

      public void Draw(Level _, GraphicsDevice graphics, SpriteBatch spriteBatch, Camera cam)
      {
        Radiation radiation = (GameMain.GameSession?.GameMode as CampaignMode)?.Map?.Radiation;
        if (radiation is null) return;

        float time = (float)(Timing.TotalTime / 50.0f);

        float brightness = Math.Clamp(Mod.CurrentModel.WorldPosRadAmountCalculator.CalculateAmount(
          radiation,
          new Vector2(
            cam.Position.X,
            cam.Position.Y //TODO it's probably inverted, need to check it
          )) * Model.Settings.RadiationToAmbienceBrightness,
          0, Model.Settings.MaxAmbienceBrightness
        );

        float rad = brightness - PerlinNoise.GetPerlin(time, time * 0.5f) * Model.Settings.AmbienceNoiseAmplitude;

        GameMain.LightManager.AmbientLight = GameMain.LightManager.AmbientLight.Add(Model.Settings.ActualColor.Multiply(rad));
      }
    }
  }

}