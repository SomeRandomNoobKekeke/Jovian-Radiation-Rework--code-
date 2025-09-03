using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Barotrauma.Networking;
using Barotrauma.Extensions;
using System.Globalization;
using MoonSharp.Interpreter;
using Barotrauma.Abilities;

namespace JovianRadiationRework
{
  public interface ICharacterHealth
  {
    public ICharacter Character { get; }
    public float GetAfflictionStrengthByIdentifier(Identifier afflictionIdentifier, bool allowLimbAfflictions = true);
    public void ApplyAffliction(ILimb targetLimb, Affliction affliction, bool allowStacking = true, bool ignoreUnkillability = false, bool recalculateVitality = true);
  }

  public class CharacterHealthProxy : ICharacterHealth
  {
    public ICharacter Character { get; }
    public bool Unkillable => characterHealth.Unkillable;
    public bool DoesBleed => characterHealth.DoesBleed;
    public List<ILimbHealth> limbHealths { get; }
    public Dictionary<Affliction, LimbHealth> afflictions => characterHealth.afflictions;

    public float GetAfflictionStrengthByIdentifier(Identifier afflictionIdentifier, bool allowLimbAfflictions = true)
      => characterHealth.GetAfflictionStrengthByIdentifier(afflictionIdentifier, allowLimbAfflictions);

    public float GetAfflictionStrengthByType(Identifier afflictionType, bool allowLimbAfflictions = true)
      => characterHealth.GetAfflictionStrengthByType(afflictionType, allowLimbAfflictions);

    public void AddAffliction(Affliction newAffliction, bool allowStacking = true)
    {
      AddLimbAffliction(limbHealth: null, limb: null, newAffliction, allowStacking);
    }

    public void AddLimbAffliction(ILimb limb, Affliction newAffliction, bool allowStacking = true, bool recalculateVitality = true)
    {
      if (!newAffliction.Prefab.LimbSpecific || limb == null) { return; }
      if (limb.HealthIndex < 0 || limb.HealthIndex >= limbHealths.Count)
      {
        DebugConsole.ThrowError("Limb health index out of bounds. Character\"" + Character.Name +
            "\" only has health configured for" + limbHealths.Count + " limbs but the limb " + limb.type + " is targeting index " + limb.HealthIndex);
        return;
      }
      AddLimbAffliction(limbHealths[limb.HealthIndex], limb, newAffliction, allowStacking, recalculateVitality);
    }


    private void AddLimbAffliction(ILimbHealth limbHealth, ILimb limb, Affliction newAffliction, bool allowStacking = true, bool recalculateVitality = true)
    {
      LimbType limbType = limb?.type ?? LimbType.None;
      if (Character.Params.IsMachine && !newAffliction.Prefab.AffectMachines) { return; }
      if (!DoesBleed && newAffliction is AfflictionBleeding) { return; }
      if (!Character.NeedsOxygen && newAffliction.Prefab == AfflictionPrefab.OxygenLow) { return; }
      if (Character.Params.Health.StunImmunity && newAffliction.Prefab.AfflictionType == AfflictionPrefab.StunType)
      {
        if (Character.EmpVulnerability <= 0 || GetAfflictionStrengthByType(AfflictionPrefab.EMPType, allowLimbAfflictions: false) <= 0)
        {
          return;
        }
      }
      if (Character.Params.Health.PoisonImmunity)
      {
        if (newAffliction.Prefab.AfflictionType == AfflictionPrefab.PoisonType || newAffliction.Prefab.AfflictionType == AfflictionPrefab.ParalysisType)
        {
          return;
        }
      }
      if (Character.EmpVulnerability <= 0 && newAffliction.Prefab.AfflictionType == AfflictionPrefab.EMPType) { return; }
      if (newAffliction.Prefab.TargetSpecies.Any() && newAffliction.Prefab.TargetSpecies.None(s => s == Character.SpeciesName)) { return; }
      if (Character.Params.Health.ImmunityIdentifiers.Contains(newAffliction.Identifier)) { return; }

      var should = GameMain.LuaCs.Hook.Call<bool?>("character.applyAffliction", this, limbHealth, newAffliction, allowStacking);

      if (should != null && should.Value)
        return;

      Affliction existingAffliction = null;
      foreach ((Affliction affliction, CharacterHealth.LimbHealth value) in afflictions)
      {
        if (value == limbHealth && affliction.Prefab == newAffliction.Prefab)
        {
          existingAffliction = affliction;
          break;
        }
      }

      if (existingAffliction != null)
      {
        float newStrength = newAffliction.Strength * (100.0f / MaxVitality) * (1f - GetResistance(existingAffliction.Prefab, limbType));
        if (allowStacking)
        {
          // Add the existing strength
          newStrength += existingAffliction.Strength;
        }
        newStrength = Math.Min(existingAffliction.Prefab.MaxStrength, newStrength);
        existingAffliction.Strength = newStrength;
        //set stun after setting the strength, because stun multipliers might want to set the strength to something else
        if (existingAffliction == stunAffliction) { Character.SetStun(newStrength, allowStunDecrease: true, isNetworkMessage: true); }
        existingAffliction.Duration = existingAffliction.Prefab.Duration;
        if (newAffliction.Source != null) { existingAffliction.Source = newAffliction.Source; }
        if (recalculateVitality)
        {
          RecalculateVitality();
        }
        return;
      }

      //create a new instance of the affliction to make sure we don't use the same instance for multiple characters
      //or modify the affliction instance of an Attack or a StatusEffect
      var copyAffliction = newAffliction.Prefab.Instantiate(
          Math.Min(newAffliction.Prefab.MaxStrength, newAffliction.Strength * (100.0f / MaxVitality) * (1f - GetResistance(newAffliction.Prefab, limbType))),
          newAffliction.Source);
      afflictions.Add(copyAffliction, limbHealth);
      AchievementManager.OnAfflictionReceived(copyAffliction, Character);
      MedicalClinic.OnAfflictionCountChanged(Character);

      Character.HealthUpdateInterval = 0.0f;

      if (recalculateVitality)
      {
        RecalculateVitality();
      }
#if CLIENT
      if (OpenHealthWindow != this && limbHealth != null)
      {
          selectedLimbIndex = -1;
      }
#endif
    }

    public void ApplyAffliction(ILimb targetLimb, Affliction affliction, bool allowStacking = true, bool ignoreUnkillability = false, bool recalculateVitality = true)
    {
      if (Character.GodMode) { return; }
      if (!ignoreUnkillability)
      {
        if (!affliction.Prefab.IsBuff && Unkillable) { return; }
      }
      if (affliction.Prefab.LimbSpecific)
      {
        if (targetLimb == null)
        {
          //if a limb-specific affliction is applied to no specific limb, apply to all limbs
          foreach (ILimbHealth limbHealth in limbHealths)
          {
            AddLimbAffliction(limbHealth, limb: null, affliction, allowStacking: allowStacking, recalculateVitality: recalculateVitality);
          }

        }
        else
        {
          AddLimbAffliction(targetLimb, affliction, allowStacking: allowStacking, recalculateVitality: recalculateVitality);
        }
      }
      else
      {
        AddAffliction(affliction, allowStacking: allowStacking);
      }
    }


    private CharacterHealth characterHealth;
    public CharacterHealthProxy(CharacterHealth characterHealth, ICharacter character)
    {
      this.characterHealth = characterHealth;
      this.Character = character;
      limbHealths = new List<ILimbHealth>(characterHealth.limbHealths.Select(lh => new LimbHealthProxy(lh)));
      afflictions = new Dictionary<Affliction, ILimbHealth>();
    }
  }

}