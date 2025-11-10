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
  public partial class ProgressiveMonsterSpawningModel : RadiationModel
  {
    public class ModelSettings : IConfig
    {
      public float TooMuchEvenForMonsters { get; set; } = -1;
      public float RadiationToMonstersMult { get; set; } = 0.002f;
      public float MaxRadiationToMonstersMult { get; set; } = 2.0f;
    }

    public override IMonsterSpawner MonsterSpawner { get; set; }
  }



}