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
    public List<string> AspectNames = new();

    public IModelAspect GetAspect(string name)
      => this.GetType()
        .GetProperty(name, BindingFlags.Public | BindingFlags.Instance)
        .GetValue(this) as IModelAspect;

    public void SetAspect(string name, IModelAspect value)
      => this.GetType()
        .GetProperty(name, BindingFlags.Public | BindingFlags.Instance)
        .SetValue(this, value);

    public IEnumerable<IModelAspect> Aspects =>
      AspectNames.Select(name => GetAspect(name));

    //Cursed
    // public IModelAspect this[string name]
    // {
    //   get => GetAspect(name);
    //   set => SetAspect(name, value);
    // }

    public bool IsComplete => !Aspects.Any(aspect => aspect is null);

    public virtual IStepsCalculator WorldProgressStepsCalculator { get; set; }
    public virtual IStepsCalculator RadiationStepsCalculator { get; set; }
    public virtual IRadiationMover RadiationMover { get; set; }
    public virtual ILocationTransformer LocationTransformer { get; set; }
    public virtual ILocationIsCriticallyRadiated LocationIsCriticallyRadiated { get; set; }
    public virtual ICharacterDamager CharacterDamager { get; set; }
    public virtual IRadiationUpdater RadiationUpdater { get; set; }
    public virtual IRadUpdateCondition RadUpdateCondition { get; set; }
    public virtual IEntityRadAmountCalculator EntityRadAmountCalculator { get; set; }
    public virtual IWorldPosRadAmountCalculator WorldPosRadAmountCalculator { get; set; }
    public virtual IMonsterSpawner MonsterSpawner { get; set; }

    //TODO optimize
    public void Combine(RadiationModel other)
    {
      foreach (string name in AspectNames)
      {
        if (other.GetAspect(name) is not null)
        {
          this.SetAspect(name, other.GetAspect(name));
        }
      }
    }

    //TODO wait, why is it instance method?
    private void ScanAspects()
    {
      AspectNames = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(pi => pi.PropertyType.IsAssignableTo(typeof(IModelAspect)))
        .Select(pi => pi.Name)
        .ToList();
    }

    private void Vitalize()
    {
      foreach (IModelAspect aspect in Aspects)
      {
        aspect?.AcceptModel(this);
      }
    }

    public RadiationModel()
    {
      ScanAspects();
      Vitalize();
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();

      bool isComplete = true;

      sb.Append("{\n");
      foreach (string name in AspectNames)
      {
        IModelAspect aspect = GetAspect(name);
        if (aspect is null) isComplete = false;
        sb.Append($"    {name}: [{Logger.WrapInColor(aspect?.GetType().Name, "white")}],\n");
      }
      sb.Append("} ");
      sb.Append($"[{(isComplete ? "Complete" : "Incomplete")}]");

      return sb.ToString();
    }

  }



}