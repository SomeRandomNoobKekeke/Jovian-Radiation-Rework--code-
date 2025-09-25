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
  public partial class ProgressiveCharacterDamagerModel : RadiationModel
  {
    public partial class ProgressiveCharacterDamagerSettings : IConfig
    {
      public float RadAmountToDPS { get; set; } = 0.01f;
      public float DamageInterval { get; set; } = 1.0f;
      public AfflictionWrapper Affliction { get; set; } = new AfflictionWrapper(AfflictionPrefab.Bleeding);
    }

    public ProgressiveCharacterDamagerSettings Settings => Mod.Config.ProgressiveCharacterDamagerSettings;

    public override ICharacterDamager CharacterDamager { get; set; } = new ProgressiveCharacterDamager();
    public override IRadUpdateCondition RadUpdateCondition { get; set; } = new ProgressiveRadUpdateCondition();
  }



}