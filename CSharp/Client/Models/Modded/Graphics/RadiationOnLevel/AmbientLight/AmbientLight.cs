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
    public partial class AmbientLightSettings : IConfig
    {
      public float RadiationToAmbienceBrightness { get; set; } = 0.02f;
      public float MaxAmbienceBrightness { get; set; } = 1.0f;
      public float AmbienceNoiseAmplitude { get; set; } = 0.5f;
      public Color ActualColor { get; set; } = new Color(0, 0, 255);
    }


    public override ILevelPostDrawer LevelPostDrawer { get; set; } = new LevelPostDrawerAspect();
  }



}