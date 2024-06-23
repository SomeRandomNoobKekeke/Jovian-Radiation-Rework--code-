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
    public class MyRadiationParams //: INetSerializableStruct
    {
      public int CriticalRadiationThreshold { get; set; } = 0;
      public int MinimumOutpostAmount { get; set; } = 3;
      public float StartingRadiation { get; set; } = -200f;
      public float RadiationStep { get; set; } = 100f;
      public float AnimationSpeed { get; set; } = 3f;
      public float RadiationDamageDelay { get; set; } = 10f;
      public float RadiationDamageAmount { get; set; } = 1f;
      public float MaxRadiation { get; set; } = -1.0f;
      public float BorderAnimationSpeed { get; set; } = 16.66f;
      public string RadiationAreaColor { get; set; } = "0,16,32,160";
      public string RadiationBorderTint { get; set; } = "0,127,255,200";

      public MyRadiationParams() { }

      public void apply()
      {
        if (GameMain.GameSession?.Map?.Radiation?.Params == null) { err("can't apply"); return; }

        foreach (PropertyInfo prop in typeof(MyRadiationParams).GetProperties())
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

        info("settings applied");
      }
    }
  }
}