using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JovianRadiationRework
{
  public partial class AmbientLightModel
  {
    public partial class ModelSettings : IConfig
    {
      public float RadiationToAmbienceBrightness { get; set; } = 0.02f;
      public float MaxAmbienceBrightness { get; set; } = 2.0f;
      public float AmbienceNoiseAmplitude { get; set; } = 0.5f;
      public Color ActualColor { get; set; } = new Color(0, 0, 255);
      public float PerlinNoiseFrequency { get; set; } = 0.04f;
    }


    public override ILevelPostDrawer LevelPostDrawer { get; set; }
  }



}