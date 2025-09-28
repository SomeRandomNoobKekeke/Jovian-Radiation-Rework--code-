using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  /// <summary>
  /// Only real props
  /// </summary>
  public class RadiationParamsFacade
  {
    private RadiationParams Params => GameMain.GameSession?.Map?.Radiation?.Params;

    public float StartingRadiation
    {
      get => Params.StartingRadiation;
      set => Params.StartingRadiation = value;
    }
    public float RadiationStep
    {
      get => Params.RadiationStep;
      set => Params.RadiationStep = value;
    }
    public float RadiationEffectMultipliedPerPixelDistance
    {
      get => Params.RadiationEffectMultipliedPerPixelDistance;
      set => Params.RadiationEffectMultipliedPerPixelDistance = value;
    }
    public int CriticalRadiationThreshold
    {
      get => Params.CriticalRadiationThreshold;
      set => Params.CriticalRadiationThreshold = value;
    }
    public int MinimumOutpostAmount
    {
      get => Params.MinimumOutpostAmount;
      set => Params.MinimumOutpostAmount = value;
    }
    public float RadiationDamageDelay
    {
      get => Params.RadiationDamageDelay;
      set => Params.RadiationDamageDelay = value;
    }
    public float RadiationDamageAmount
    {
      get => Params.RadiationDamageAmount;
      set => Params.RadiationDamageAmount = value;
    }
    public float MaxRadiation
    {
      get => Params.MaxRadiation;
      set => Params.MaxRadiation = value;
    }
    public float AnimationSpeed
    {
      get => Params.AnimationSpeed;
      set => Params.AnimationSpeed = value;
    }
    // public Color RadiationAreaColor
    // {
    //   get => Params.RadiationAreaColor;
    //   set => Params.RadiationAreaColor = value;
    // }
    // public Color RadiationBorderTint
    // {
    //   get => Params.RadiationBorderTint;
    //   set => Params.RadiationBorderTint = value;
    // }
    public string RadiationAreaColor
    {
      get => XMLExtensions.ColorToString(Params.RadiationAreaColor);
      set => Params.RadiationAreaColor = XMLExtensions.ParseColor(value);
    }
    public string RadiationBorderTint
    {
      get => XMLExtensions.ColorToString(Params.RadiationBorderTint);
      set => Params.RadiationBorderTint = XMLExtensions.ParseColor(value);
    }
    public float BorderAnimationSpeed
    {
      get => Params.BorderAnimationSpeed;
      set => Params.BorderAnimationSpeed = value;
    }
  }

  public class RadiationParamsAccess : ReflectionProxy
  {
    public static Dictionary<string, string> DefaultValues = new() //HACK
    {
      ["StartingRadiation"] = "-100",
      ["RadiationStep"] = "100",
      ["RadiationEffectMultipliedPerPixelDistance"] = "250",
      ["CriticalRadiationThreshold"] = "10",
      ["MinimumOutpostAmount"] = "3",
      ["RadiationDamageDelay"] = "10",
      ["RadiationDamageAmount"] = "1",
      ["BorderAnimationSpeed"] = "-1",
      ["MaxRadiation"] = "16.66",
      ["AnimationSpeed"] = "3",
      ["RadiationAreaColor"] = "139,0,0,85",
      ["RadiationBorderTint"] = "255,0,0,255",
      ["BorderAnimationSpeed"] = "16.66",
    };

    public void Reset()
    {
      foreach (var (key, value) in DefaultValues) Set(key, value);
    }

    public static RadiationParamsAccess Instance = new RadiationParamsAccess();
    public RadiationParamsAccess() : base(new RadiationParamsFacade()) { }
  }



}