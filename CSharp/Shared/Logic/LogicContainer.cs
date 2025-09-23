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
  public class LogicContainer
  {
    public IStepsCalculator WorldProgressStepsCalculator { get; set; } = new VanillaStepsCalculator();
    public IStepsCalculator RadiationStepsCalculator { get; set; } = new VanillaStepsCalculator();
    public IRadiationMover RadiationMover { get; set; } = new VanillaRadiationMover();
    public ILocationTransformer LocationTransformer { get; set; } = new VanillaLocationTransformer();
    public ICharacterDamager CharacterDamager { get; set; } = new VanillaCharacterDamager();
    public IRadiationUpdater RadiationUpdater { get; set; } = new VanillaRadiationUpdater();
    public IRadAmountCalculator RadAmountCalculator { get; set; } = new VanillaRadAmountCalculator();
  }



}