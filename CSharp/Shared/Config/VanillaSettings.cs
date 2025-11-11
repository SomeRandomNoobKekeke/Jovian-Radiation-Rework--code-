using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using BaroJunk;



namespace JovianRadiationRework
{
  public class VanillaSettings : IConfig
  {
    public RadiationParamsFacade ParamsFacade;
    public ReflectionProxy Params;
    public ReflectionProxy Own;

    public bool OverrideVanillaSettings { get; set; } = true;

    public float StartingRadiation { get; set; } = -100.0f;
    public float RadiationStep { get; set; } = 100.0f;
    public float RadiationEffectMultipliedPerPixelDistance { get; set; } = 250.0f;
    public int CriticalRadiationThreshold { get; set; } = 10;
    public int MinimumOutpostAmount { get; set; } = 3;
    public float RadiationDamageDelay { get; set; } = 10.0f;
    public float RadiationDamageAmount { get; set; } = 1.0f;
    public float MaxRadiation { get; set; } = -1.0f;
    public float AnimationSpeed { get; set; } = 3.0f;
    public string RadiationAreaColor { get; set; } = "139,0,0,85";
    public string RadiationBorderTint { get; set; } = "255,0,0,255";
    public float BorderAnimationSpeed { get; set; } = 16.66f;

    public void Apply()
    {
      if (!OverrideVanillaSettings) return;
      if (!ParamsFacade.RadiationEnabled) return;

      foreach (string name in Own.GetPropNames())
      {
        Params.Set(name, Own.Get(name).ToString());
      }
    }

    public event Action<float> StartingRadiationSet;

    public VanillaSettings()
    {
      ParamsFacade = new RadiationParamsFacade();
      Params = new ReflectionProxy(ParamsFacade);
      Own = new ReflectionProxy(this);

      this.OnPropChanged((key, value) =>
      {
        if (key == "StartingRadiation")
        {
          StartingRadiationSet?.Invoke((float)value);
        }
      });

      this.OnUpdated(() =>
      {
        Apply();
        StartingRadiationSet?.Invoke(StartingRadiation);
      });
    }
  }
}