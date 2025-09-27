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
  public partial class DamageToElectronicsModel : RadiationModel
  {
    public partial class ModelSettings : IConfig
    {
      public float DamageInterval { get; set; } = 1.0f;
      public float Damage { get; set; } = 1.0f;
      public float MaxDamage { get; set; } = 10.0f;
    }

    public override IElectronicsDamager ElectronicsDamager { get; set; }
  }



}