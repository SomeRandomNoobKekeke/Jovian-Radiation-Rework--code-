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
    [ReflectionProxy.NotAProp]
    public RadiationParams Params => GameMain.GameSession?.Map?.Radiation?.Params;
    [ReflectionProxy.NotAProp]
    public bool RadiationEnabled => Params != null;

    public float StartingRadiation
    {
      get => Params?.StartingRadiation ?? default;
      set { if (Params is not null) Params.StartingRadiation = value; }
    }
    public float RadiationStep
    {
      get => Params?.RadiationStep ?? default;
      set { if (Params is not null) Params.RadiationStep = value; }
    }
    public float RadiationEffectMultipliedPerPixelDistance
    {
      get => Params?.RadiationEffectMultipliedPerPixelDistance ?? default;
      set { if (Params is not null) Params.RadiationEffectMultipliedPerPixelDistance = value; }
    }
    public int CriticalRadiationThreshold
    {
      get => Params?.CriticalRadiationThreshold ?? default;
      set { if (Params is not null) Params.CriticalRadiationThreshold = value; }
    }
    public int MinimumOutpostAmount
    {
      get => Params?.MinimumOutpostAmount ?? default;
      set { if (Params is not null) Params.MinimumOutpostAmount = value; }
    }
    public float RadiationDamageDelay
    {
      get => Params?.RadiationDamageDelay ?? default;
      set { if (Params is not null) Params.RadiationDamageDelay = value; }
    }
    public float RadiationDamageAmount
    {
      get => Params?.RadiationDamageAmount ?? default;
      set { if (Params is not null) Params.RadiationDamageAmount = value; }
    }
    public float MaxRadiation
    {
      get => Params?.MaxRadiation ?? default;
      set { if (Params is not null) Params.MaxRadiation = value; }
    }
    public float AnimationSpeed
    {
      get => Params?.AnimationSpeed ?? default;
      set { if (Params is not null) Params.AnimationSpeed = value; }
    }
    public string RadiationAreaColor
    {
      get => Params is null ? "0,0,0,0" : XMLExtensions.ColorToString(Params.RadiationAreaColor);
      set { if (Params is not null) Params.RadiationAreaColor = XMLExtensions.ParseColor(value); }
    }
    public string RadiationBorderTint
    {
      get => Params is null ? "0,0,0,0" : XMLExtensions.ColorToString(Params.RadiationBorderTint);
      set { if (Params is not null) Params.RadiationBorderTint = XMLExtensions.ParseColor(value); }
    }
    public float BorderAnimationSpeed
    {
      get => Params?.BorderAnimationSpeed ?? default;
      set { if (Params is not null) Params.BorderAnimationSpeed = value; }
    }
  }

  public class RadiationParamsAccess : ReflectionProxy
  {
    public static RadiationParamsAccess Instance = new RadiationParamsAccess();
    public static Dictionary<string, string> DefaultValues = new() //HACK
    {
      ["StartingRadiation"] = "-100",
      ["RadiationStep"] = "100",
      ["RadiationEffectMultipliedPerPixelDistance"] = "250",
      ["CriticalRadiationThreshold"] = "10",
      ["MinimumOutpostAmount"] = "3",
      ["RadiationDamageDelay"] = "10",
      ["RadiationDamageAmount"] = "1",
      ["MaxRadiation"] = "-1.0",
      ["AnimationSpeed"] = "3",
      ["RadiationAreaColor"] = "139,0,0,85",
      ["RadiationBorderTint"] = "255,0,0,255",
      ["BorderAnimationSpeed"] = "16.66",
    };

    public void Reset()
    {
      foreach (var (key, value) in DefaultValues) Set(key, value);
    }

    public RadiationParamsAccess() : base(new RadiationParamsFacade()) { }

    public override string ToString()
    {
      //CRINGE is spreading
      return Logger.Wrap.AsJson(Target);
    }
  }



}