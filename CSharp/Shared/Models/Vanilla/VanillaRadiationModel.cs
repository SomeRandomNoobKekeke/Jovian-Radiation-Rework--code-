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
    public override IStepsCalculator WorldProgressStepsCalculator { get; set; } = new VanillaStepsCalculator();
    public override IStepsCalculator RadiationStepsCalculator { get; set; } = new VanillaStepsCalculator();
    public override IRadiationMover RadiationMover { get; set; } = new VanillaRadiationMover();
    public override ILocationTransformer LocationTransformer { get; set; } = new VanillaLocationTransformer();
    public override ICharacterDamager CharacterDamager { get; set; } = new VanillaCharacterDamager();
    public override IRadiationUpdater RadiationUpdater { get; set; } = new VanillaRadiationUpdater();
    public override IEntityRadAmountCalculator EntityRadAmountCalculator { get; set; } = new VanillaEntityRadAmountCalculator();
    public override IWorldPosRadAmountCalculator WorldPosRadAmountCalculator { get; set; } = new VanillaWorldPosRadAmountCalculator();
    public override IMonsterSpawner MonsterSpawner { get; set; } = new VanillaMonsterSpawner();
  }



}