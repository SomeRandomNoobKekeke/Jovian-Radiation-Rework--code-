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
  public partial class CustomMonsterDamagerModel : RadiationModel
  {
    public partial class ModelSettings : IConfig
    {
      public bool BuffMonsters { get; set; } = true;
    }

    public override bool Debug { get; set; } = false;
    public override IMonsterDamager MonsterDamager { get; set; }
  }



}