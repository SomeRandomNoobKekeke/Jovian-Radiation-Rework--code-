using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.IO;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Barotrauma.Extensions;
using Barotrauma.Networking;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace JovianRadiationRework
{
  public partial class Mod
  {
    public struct ModMetadata
    {
      public string ModVersion { get; set; } = "1.0.0";
      public string ModName { get; set; } = "Jovian Radiation Rework";
      public string ModDir { get; set; } = "";

      public ModMetadata() { }
    }

    public class ProgressSettings
    {
      public float WorldProgressStepDuration { get; set; } = 10.0f;
      public float WorldProgressMaxStepsPerRound { get; set; } = 5;
      public bool KeepSurroundingOutpostsAlive { get; set; } = true;
      public bool SmoothProgress { get; set; } = true;
      public bool ProgressOnSaveLoad { get; set; } = true;
      public float GracePeriod { get; set; } = 1.0f;
      public float RadiationSlowDown { get; set; } = 0.0075f;
    }

    //[NetworkSerialize]
    public class ModSettings //: INetSerializableStruct
    {
      public float WaterRadiationBlockPerMeter { get; set; } = 0.6f;
      public float RadiationDamage { get; set; } = 0.0275f;

      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public float? RadiationSlowDown { get; set; }
      public float TooMuchEvenForMonsters { get; set; } = 400;
      public float FractionOfRadiationBlockedInSub { get; set; } = 0.5f;
      public float HuskRadiationResistance { get; set; } = 0.5f;
      public float RadiationToAmbienceBrightness { get; set; } = 0.00075f;
      public float MaxAmbienceBrightness { get; set; } = 0.4f;
      public string AmbienceColor { get; set; } = "0,255,255";
      [JsonIgnore] public Color ActualColor { get; set; } = new Color(0, 255, 255);

      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public float? WorldProgressStepDuration { get; set; }

      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public float? WorldProgressMaxStepsPerRound { get; set; }
      public ProgressSettings Progress { get; set; } = new ProgressSettings();
      public bool UseVanillaRadiation { get; set; } = false;
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public bool? KeepSurroundingOutpostsAlive { get; set; }

      public ModSettings() { }
    }
  }
}