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
      /// <summary>
      /// So ~ 1 full time default engineer can maintain dugong at 400 camera irradiation
      /// it's ~ 0.2 on jrc inside sub 
      /// So beyound 0.2 on jrc you need hazmats suit inside sub / extra engineers / electric upgrades
      /// </summary>
      public float RadAmountToDPS { get; set; } = 0.002f;
      public float MaxDPS { get; set; } = 1.5f;
    }

    public override IElectronicsDamager ElectronicsDamager { get; set; }
  }



}