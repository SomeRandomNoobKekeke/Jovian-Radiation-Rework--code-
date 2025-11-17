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
  public partial class CustomCharacterDamagerModel : RadiationModel
  {
    public partial class ModelSettings : IConfig
    {
      public float RadAmountToDPS { get; set; } = 0.001f;

      public AfflictionWrapper Affliction { get; set; } = new AfflictionWrapper(AfflictionPrefab.RadiationSickness);
    }

    public override bool Debug { get; set; } = false;
    public override IHumanDamager HumanDamager { get; set; }
  }



}