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
    // for normal (1.0 vitality)
    // Values are not 100% precise, tested on real mission
    public const float NaturalRegen = 0.20f;
    public const float MaxTolerableInDivingSuit = 0.27f;
    public const float MaxTolerableInHazmatSuit = 0.25f;
    public const float MaxTolerableInDivingSuitAndHazmat = 0.33f;
    public const float MaxTolerableInPUCS = 0.80f;
    public const float MaxTolerableInPUCSAndHazmat = 1.0f;

    // For abyssal (0.9 vitality) multiply by 0.9
    // for easy (1.1 vitality) multiply by 1.1

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
          >= MaxTolerableInDivingSuitAndHazmat => "3",
          >= NaturalRegen and < MaxTolerableInDivingSuitAndHazmat => "2",
          > 0 and < NaturalRegen => "1",
          0 => "0",
        };

        if (customInterface.uiElements.ElementAtOrDefault(1) is GUITextBox textBox)
        {
          textBox.Text = dps.ToString("0.0000");
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