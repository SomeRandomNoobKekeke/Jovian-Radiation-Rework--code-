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
  public partial class CustomRadiationUpdaterModel : RadiationModel
  {
    public partial class ModelSettings : IConfig
    {
      public float CharacterDamageInterval { get; set; } = 5.0f;
    }

    public override IRadiationUpdater RadiationUpdater { get; set; }
  }
}