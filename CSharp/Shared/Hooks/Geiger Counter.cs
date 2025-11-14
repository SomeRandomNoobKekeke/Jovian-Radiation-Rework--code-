global using BaroJunk;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Barotrauma.Items.Components;

namespace JovianRadiationRework
{
  public static class GeigerCounterHooks
  {
    // for easy (1.1 vitality)
    // public const float NaturalRegen = 0.22f;
    // public const float MaxTolerableInHazmatSuit = 0.54f;
    // public const float MaxTolerableInPUCS = 0.88f;
    // public const float MaxTolerableInPUCSAndHazmat = 2.25f;

    // For abyssal (0.9 vitality)
    // public const float NaturalRegen = 0.17f;
    // public const float MaxTolerableInHazmatSuit = 0.45f;
    // public const float MaxTolerableInPUCS = 0.72f;
    // public const float MaxTolerableInPUCSAndHazmat = 1.8f;


    // for normal (1.0 vitality)
    // Values are not 100% precise, tested on real mission
    public const float NaturalRegen = 0.20f;
    public const float MaxTolerableInHazmatSuit = 0.50f;
    public const float MaxTolerableInPUCS = 0.80f;
    public const float MaxTolerableInPUCSAndHazmat = 2.0f;

    public static object MeasureRadiation(object[] args)
    {
      if (GameMain.GameSession?.Map?.Radiation?.Enabled != true) return null;

      if (args.ElementAtOrDefault(2) is Item item)
      {
        LightComponent lightComponent = item.GetComponent<LightComponent>();
        CustomInterface customInterface = item.GetComponent<CustomInterface>();

        float dps = Mod.CurrentModel.WorldPosRadAmountCalculator.RadAmountToRadDps(
          Mod.CurrentModel.WorldPosRadAmountCalculator.CalculateAmountForItem(
            GameMain.GameSession.Map.Radiation, item
          )
        );

        dps = Math.Max(0, dps);

        lightComponent.Msg = dps switch
        {
          >= MaxTolerableInHazmatSuit => "3",
          >= NaturalRegen and < MaxTolerableInHazmatSuit => "2",
          > 0 and < NaturalRegen => "1",
          0 => "0",
        };

        if (customInterface.uiElements.ElementAtOrDefault(1) is GUITextBox textBox)
        {
          textBox.Text = $"{dps}";
        }
      }
      return null;
    }

    public static object OnTurnedOff(object[] args)
    {
      if (GameMain.GameSession?.Map?.Radiation?.Enabled != true) return null;

      if (args.ElementAtOrDefault(2) is Item item)
      {
        LightComponent lightComponent = item.GetComponent<LightComponent>();
        CustomInterface customInterface = item.GetComponent<CustomInterface>();

        lightComponent.Msg = "0";

        if (customInterface.uiElements.ElementAtOrDefault(1) is GUITextBox textBox)
        {
          textBox.Text = "";
        }
      }
      return null;
    }
  }


}