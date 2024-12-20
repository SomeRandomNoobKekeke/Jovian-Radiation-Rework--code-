using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;


namespace JovianRadiationRework
{
  public class ModSettings
  {
    public ProgressSettings Progress { get; set; } = new ProgressSettings();

    public float WaterRadiationBlockPerMeter { get; set; } = 0.6f;
    public float RadiationDamage { get; set; } = 0.0275f;
    public float TooMuchEvenForMonsters { get; set; } = 600;
    public float FractionOfRadiationBlockedInSub { get; set; } = 0.5f;
    public float HuskRadiationResistance { get; set; } = 0.5f;
    public float RadiationToAmbienceBrightness { get; set; } = 0.001f;
    public float MaxAmbienceBrightness { get; set; } = 1.0f;
    public float AmbienceNoiseAmplitude { get; set; } = 0.5f;
    public string AmbienceColor { get; set; } = "0,255,255";
    public bool UseVanillaRadiation { get; set; } = false;
    [Ignore] public Color ActualColor { get; set; } = new Color(0, 255, 255);
    public float ElectronicsDamageMultiplier { get; set; } = 0.008f;
    public float MaxDamageToElectronics { get; set; } = 6.0f;
    public float RadiationToMonstersMult { get; set; } = 0.005f;
    public float MaxRadiationToMonstersMult { get; set; } = 2.0f;

    public ModSettings() { }
  }
}