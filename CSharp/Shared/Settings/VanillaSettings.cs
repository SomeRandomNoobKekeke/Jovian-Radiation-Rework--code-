using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;


namespace JovianRadiationRework
{
  public struct VanillaSettings
  {
    public static FlatView flatView = new FlatView(typeof(VanillaSettings));
    public static FlatView vanillaFlatView = new FlatView(typeof(RadiationParams));

    public static Dictionary<Type, Func<object, object>> SpecialTransform = new Dictionary<Type, Func<object, object>>(){
      {typeof(Color), (value) => XMLExtensions.ParseColor((string)value)}
    };

    public int CriticalRadiationThreshold { get; set; } = 3;
    public int MinimumOutpostAmount { get; set; } = 3;
    public float StartingRadiation { get; set; } = 0f;
    public float RadiationStep { get; set; } = 20f;
    public float AnimationSpeed { get; set; } = 3f;
    public float RadiationDamageDelay { get; set; } = 10f;
    public float RadiationDamageAmount { get; set; } = 1f;
    public float MaxRadiation { get; set; } = -1.0f;
    public float BorderAnimationSpeed { get; set; } = 16.66f;
    public string RadiationAreaColor { get; set; } = "0,16,32,160";
    public string RadiationBorderTint { get; set; } = "0,127,255,200";

    public void Apply()
    {
      if (GameMain.GameSession?.Map?.Radiation?.Params == null)
      {
        string screen = $"on {Screen.Selected}";
        string gameMode = GameMain.GameSession == null ? "" : $"in {GameMain.GameSession.GameMode}";
        Mod.Info($"Can't apply vanilla radiation settings {screen} {gameMode}");
        return;
      }

      foreach (string key in vanillaFlatView.Props.Keys)
      {
        Mod.Log($"{key} {vanillaFlatView.Props[key]}");
      }

      if (GameMain.GameSession?.Campaign.IsFirstRound == true)
      {
        GameMain.GameSession.Map.Radiation.Amount = StartingRadiation;
      }

      Mod.Info("settings applied");
    }

    public VanillaSettings() { }
  }
}