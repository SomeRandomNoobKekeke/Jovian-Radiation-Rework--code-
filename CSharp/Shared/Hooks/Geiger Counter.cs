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
    public static float NaturalRegen = 0.2f;
    public static float MaxTolerableInHazmatSuit = 0.33f;
    public static float MaxTolerableInPUCS = 0.44f;

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

        lightComponent.Msg = dps switch
        {
          > 100 => "3",
          > 50 and < 100 => "2",
          > 0 and < 50 => "1",
          _ => "0",
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