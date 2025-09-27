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
  public partial class VanillaRadiationModel : RadiationModel
  {
    public override IStepsCalculator WorldProgressStepsCalculator { get; set; }
    public override IStepsCalculator RadiationStepsCalculator { get; set; }
    public override IRadiationMover RadiationMover { get; set; }
    public override ILocationTransformer LocationTransformer { get; set; }
    public override ILocationIsCriticallyRadiated LocationIsCriticallyRadiated { get; set; }
    public override ICharacterDamager CharacterDamager { get; set; }
    public override IRadiationUpdater RadiationUpdater { get; set; }
    public override IRadUpdateCondition RadUpdateCondition { get; set; }
    public override IEntityRadAmountCalculator EntityRadAmountCalculator { get; set; }
    public override IWorldPosRadAmountCalculator WorldPosRadAmountCalculator { get; set; }
    public override IMonsterSpawner MonsterSpawner { get; set; }
  }



}