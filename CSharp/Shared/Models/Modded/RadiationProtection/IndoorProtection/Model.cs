using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;



namespace JovianRadiationRework
{
  //TODO rename this and everything related
  /// <summary>
  /// A bit confusing, it's responsible for all resistances in enclosed spaces
  /// </summary>
  public partial class IndoorProtectionModel : RadiationModel
  {
    public class ModelSettings : IConfig
    {
      public float MainSub { get; set; } = 0.5f;
      public float Beacon { get; set; } = 1.0f;
      public float Outpost { get; set; } = 1.0f;
      public float EnemySub { get; set; } = 1.0f;
      public float Wreck { get; set; } = 0.75f;
      public float Ruins { get; set; } = 1.0f;

      //No way to check if entiry is in a cave, doesn't work for now 
      public float Cave { get; set; } = 0.75f;
    }

    public override bool Debug { get; set; } = false;
    public override IIndoorProtectionCalculator IndoorProtectionCalculator { get; set; }
  }



}