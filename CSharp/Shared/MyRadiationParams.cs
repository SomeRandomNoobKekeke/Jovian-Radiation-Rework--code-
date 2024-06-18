using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

using Barotrauma.Extensions;
namespace JovianRadiationRework
{
  public struct MyRadiationParams
  {
    public float StartingRadiation { get; set; } = -100f;
    public float RadiationStep { get; set; } = 100f;
    public int CriticalRadiationThreshold { get; set; } = 10;
    public int MinimumOutpostAmount { get; set; } = 3;
    public float AnimationSpeed { get; set; } = 3f;
    public float RadiationDamageDelay { get; set; } = 10f;
    public float RadiationDamageAmount { get; set; } = 1f;
    public float MaxRadiation { get; set; } = -1.0f;
    public Color RadiationAreaColor { get; set; } = new Color(139, 0, 0, 85);
    public Color RadiationBorderTint { get; set; } = new Color(255, 0, 0, 255);
    public float BorderAnimationSpeed { get; set; } = 16.66f;

    public RadiationParams() { }
  }
}