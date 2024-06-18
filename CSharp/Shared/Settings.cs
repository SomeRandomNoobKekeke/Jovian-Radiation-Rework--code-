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
  public partial class Mod
  {
    public struct ModMetadata
    {
      public string ModVersion { get; set; } = "1.0.0";
      public string ModName { get; set; } = "Jovian Radiation Rework";
      public string ModDir { get; set; } = "";

      public ModMetadata() { }
    }

    public partial struct Settings
    {
      public float WaterRadiationBlockPerMeter { get; set; } = 0.6f;
      public float RadiationDamage { get; set; } = 0.037f;
      public float TooMuchEvenForMonsters { get; set; } = 300;
      public float HuskRadiationResistance { get; set; } = 0.5f;
      public float RadiationToColor { get; set; } = 0.001f;
      public MyRadiationParams vanilla { get; set; } = new MyRadiationParams();

      public Settings() { }
    }

  }
}