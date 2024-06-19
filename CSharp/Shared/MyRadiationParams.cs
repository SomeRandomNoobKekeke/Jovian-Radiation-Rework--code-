using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

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
      [XmlAttribute] public int CriticalRadiationThreshold { get; set; } = 0;
      [XmlAttribute] public int MinimumOutpostAmount { get; set; } = 3;
      [XmlAttribute] public float StartingRadiation { get; set; } = -100f;
      [XmlAttribute] public float RadiationStep { get; set; } = 100f;
      [XmlAttribute] public float AnimationSpeed { get; set; } = 3f;
      [XmlAttribute] public float RadiationDamageDelay { get; set; } = 10f;
      [XmlAttribute] public float RadiationDamageAmount { get; set; } = 1f;
      [XmlAttribute] public float MaxRadiation { get; set; } = -1.0f;
      [XmlAttribute] public float BorderAnimationSpeed { get; set; } = 16.66f;
      [XmlAttribute] public string RadiationAreaColor { get; set; } = "0,16,32,180";
      [XmlAttribute] public string RadiationBorderTint { get; set; } = "0,127,255,255";

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

        info("settings applyed");
      }
    }
  }
}