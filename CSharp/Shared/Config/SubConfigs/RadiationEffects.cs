using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using BaroJunk;

namespace JovianRadiationRework
{
  public class RadiationEffects : IConfig
  {
    public MoreMonstersModel.ModelSettings MoreMonsters { get; set; }
    public DamageToElectronicsModel.ModelSettings DamageToElectronics { get; set; }
    public CustomCharacterDamagerModel.ModelSettings DamageToCharacters { get; set; }
    public CustomMonsterDamagerModel.ModelSettings DamageToMonsters { get; set; }
  }
}