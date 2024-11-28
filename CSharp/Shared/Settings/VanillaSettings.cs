using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;


namespace JovianRadiationRework
{
  public class VanillaSettings
  {
    public static FlatView flatView = new FlatView(typeof(VanillaSettings));
    public static FlatView vanillaFlatView = new FlatView(typeof(RadiationParams));


    public int CriticalRadiationThreshold { get; set; } = 1000;
    public int MinimumOutpostAmount { get; set; } = 3;
    public float StartingRadiation { get; set; } = -60f;
    public float RadiationStep { get; set; } = 20;
    public float AnimationSpeed { get; set; } = 2f;
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

      foreach (string key in flatView.Props.Keys)
      {
        object value = flatView.Get(this, key);

        vanillaFlatView.Set(GameMain.GameSession.Map.Radiation.Params, key, value);
      }

      if (GameMain.GameSession?.Campaign.IsFirstRound == true)
      {
        GameMain.GameSession.Map.Radiation.Amount = StartingRadiation;
      }

      Mod.Info("Radiation Settings Applied");
    }

    public VanillaSettings() { }
  }
}