using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

using Barotrauma.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Voronoi2;


namespace JovianRadiationRework
{
  public interface IModelAspect
  {
    public void AcceptSettings(IConfig settings)
      => SettingsProp?.SetValue(this, settings);

    public void AcceptModel(RadiationModel model)
      => ModelProp?.SetValue(this, model);

    public PropertyInfo SettingsProp
      => this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
          .FirstOrDefault(pi => pi.PropertyType.IsAssignableTo(typeof(IConfig)));

    //CRINGE
    public PropertyInfo ModelProp
      => this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
          .FirstOrDefault(pi => pi.PropertyType.IsAssignableTo(typeof(RadiationModel)));

    public void InjectFacades(IFacadeProvider provider)
    {
      foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
      {
        if (pi.PropertyType.IsAssignableTo(typeof(IFacade)))
        {
          pi.SetValue(this, provider.Get(pi.PropertyType));
        }
      }
    }
  }

  public interface ILifeCycleHooks : IModelAspect
  {
    public void OnLoad() { }
    public void OnRoundStart() { }
    public void OnRoundEnd() { }
    public void OnSaveAndQuit() { }
    public void OnQuit() { }
  }

  public partial interface IStepsCalculator : IModelAspect
  {
    public float CalculateSteps(CampaignMode.TransitionType transitionType, float roundDuration);
  }
  public interface IRadiationMover : IModelAspect
  {
    public void InitOnFirstRound();
    public void MoveRadiation(Radiation _, float steps);
  }

  public interface ILocationTransformer : IModelAspect
  {
    public void TransformLocations(Radiation _);
  }

  public interface ILocationIsCriticallyRadiated : IModelAspect
  {
    public bool IsIt(Location _);
  }

  public interface IHumanDamager : IModelAspect
  {
    //WHY is it here? Where should it be?
    public float RadAmountToRadDps(float amount);
    public void DamageHuman(Character character, float radAmount, Radiation _);
  }

  public interface IMonsterDamager : IModelAspect
  {
    public void DamageMonster(Character character, float radAmount, Radiation _);
  }

  public interface IElectronicsDamager : IModelAspect
  {
    public void DamageElectronics(Radiation _, float deltaTime);
    public void ScanItems();
    public void ForgetItems();
  }

  public interface IRadiationUpdater : IModelAspect
  {
    //BRUH this feels very stupid that i have to expose every setting like this
    public float GetUpdateInterval();
    public void UpdateRadiation(Radiation _, float deltaTime);
  }

  /// <summary>
  /// Note: those could be negative, it's consumers responsibility to Max(0,x) if necessary
  /// </summary>
  public interface IWorldPosRadAmountCalculator : IModelAspect
  {
    public float RadAmountToRadDps(float amount);
    public float CalculateAmount(Radiation _, Vector2 pos);
    public float CalculateAmountForCharacter(Radiation _, Character character)
      => CalculateAmount(_, character.WorldPosition);
    public float CalculateAmountForItem(Radiation _, Item item)
      => CalculateAmount(_, item.WorldPosition);
  }

  public interface IMonsterSpawner : IModelAspect
  {
    public void SpawnMonsters(MonsterEvent _);
  }
  public interface ISaveAndQuitHandler : IModelAspect
  {
    public void HandleSaveAndQuit(CampaignMode _);
  }

  public interface IMetadataSetter : IModelAspect
  {
    public void SetMetadata();
  }

  public interface IIndoorProtectionCalculator : IModelAspect
  {
    public float GetBasicMainSubProtection();
    public float GetMainsubElectronicsProtectionMult();
    public float GetHullProtectionMult(Radiation _, Entity entity);
  }
  public interface IHullUpgrades : IModelAspect
  {
    public void UpdateMainSubProtection();
    public float GetProtectionPerUpgrade();
    public float GetProtectionForMainSub();
  }
  public interface IHuskResistanceCalculator : IModelAspect
  {
    public float GetHuskResistanceMult(Radiation _, Character character);
  }

}