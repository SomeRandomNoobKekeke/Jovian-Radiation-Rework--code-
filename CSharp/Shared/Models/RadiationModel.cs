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
  public class RadiationModel
  {
    public virtual IStepsCalculator WorldProgressStepsCalculator { get; set; }
    public virtual IStepsCalculator RadiationStepsCalculator { get; set; }
    public virtual IRadiationMover RadiationMover { get; set; }
    public virtual ILocationTransformer LocationTransformer { get; set; }
    public virtual ICharacterDamager CharacterDamager { get; set; }
    public virtual IRadiationUpdater RadiationUpdater { get; set; }
    public virtual IRadAmountCalculator RadAmountCalculator { get; set; }

    //TODO report conflicts
    public void Combine(RadiationModel other)
    {
      if (other.WorldProgressStepsCalculator is not null) WorldProgressStepsCalculator = other.WorldProgressStepsCalculator;
      if (other.RadiationStepsCalculator is not null) RadiationStepsCalculator = other.RadiationStepsCalculator;
      if (other.RadiationMover is not null) RadiationMover = other.RadiationMover;
      if (other.LocationTransformer is not null) LocationTransformer = other.LocationTransformer;
      if (other.CharacterDamager is not null) CharacterDamager = other.CharacterDamager;
      if (other.RadiationUpdater is not null) RadiationUpdater = other.RadiationUpdater;
      if (other.RadAmountCalculator is not null) RadAmountCalculator = other.RadAmountCalculator;
    }
  }



}