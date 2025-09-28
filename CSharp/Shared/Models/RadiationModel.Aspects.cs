using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using System.Text;



namespace JovianRadiationRework
{
  public partial class RadiationModel
  {
    public virtual ILifeCycleHooks LifeCycleHooks { get; set; }
    public virtual IStepsCalculator WorldProgressStepsCalculator { get; set; }
    public virtual IStepsCalculator RadiationStepsCalculator { get; set; }
    public virtual IRadiationMover RadiationMover { get; set; }
    public virtual ILocationTransformer LocationTransformer { get; set; }
    public virtual ILocationIsCriticallyRadiated LocationIsCriticallyRadiated { get; set; }
    public virtual IHumanDamager HumanDamager { get; set; }
    public virtual IMonsterDamager MonsterDamager { get; set; }
    public virtual IElectronicsDamager ElectronicsDamager { get; set; }
    public virtual IRadiationUpdater RadiationUpdater { get; set; }
    public virtual IEntityRadAmountCalculator EntityRadAmountCalculator { get; set; }
    public virtual IWorldPosRadAmountCalculator WorldPosRadAmountCalculator { get; set; }
    public virtual IMonsterSpawner MonsterSpawner { get; set; }
  }
}