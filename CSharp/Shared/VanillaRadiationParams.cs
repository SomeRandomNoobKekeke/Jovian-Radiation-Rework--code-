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
    //[NetworkSerialize]
    public class VanillaRadiationParams //: INetSerializableStruct
    {
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

      public VanillaRadiationParams() { }

      public void apply()
      {
        if (GameMain.GameSession?.Map?.Radiation?.Params == null) { err("can't apply"); return; }

        foreach (PropertyInfo prop in typeof(VanillaRadiationParams).GetProperties())
        {
          PropertyInfo target = typeof(RadiationParams).GetProperty(prop.Name);
          Object value = prop.GetValue(settings.vanilla);
          if (target.PropertyType == typeof(Color))
          {
            //:AwareDev:
            // https://github.com/FakeFishGames/Barotrauma/blob/master/Barotrauma/BarotraumaShared/SharedSource/Serialization/XMLExtensions.cs#L853
            value = XMLExtensions.ParseColor((string)value);
          }
          target.SetValue(GameMain.GameSession.Map.Radiation.Params, value);
        }

        if (GameMain.GameSession?.Campaign.IsFirstRound == true)
        {
          GameMain.GameSession.Map.Radiation.Amount = StartingRadiation;
        }

        info("settings applied");
      }


      public void printRealValues()
      {
        log("Real Vanilla Radiation Settings:", Color.DeepPink);
        foreach (var p in GameMain.GameSession.Map.Radiation.Params.SerializableProperties)
        {
          log($"{p.Key} = {p.Value.GetValue(GameMain.GameSession.Map.Radiation.Params)}");
        }
      }
    }
  }
}