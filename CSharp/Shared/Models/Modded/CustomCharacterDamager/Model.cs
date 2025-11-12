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
  public partial class CustomCharacterDamager : RadiationModel
  {
    public partial class ModelSettings : IConfig
    {
      public float RadAmountToDPS { get; set; } = 0.1f;
      public float DamageInterval { get; set; } = 1.0f;
      public AfflictionWrapper Affliction { get; set; } = new AfflictionWrapper(AfflictionPrefab.Bleeding);
    }

    public override IHumanDamager HumanDamager { get; set; }
  }



}