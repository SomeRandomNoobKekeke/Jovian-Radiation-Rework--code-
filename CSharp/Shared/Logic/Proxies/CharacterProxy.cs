using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public interface ICharacter : IEntity
  {
    public bool IsDead { get; }
    public bool Removed { get; }
    public bool GodMode { get; }
    public string Name { get; }
    public bool NeedsOxygen { get; }
    public float EmpVulnerability { get; }
    public Identifier SpeciesName { get; }
    public ICharacterParams Params { get; }
    public ICharacterHealth CharacterHealth { get; }
    public IAnimController AnimController { get; }

    public void ApplyAfflictionToMainLimb(Affliction affliction);
  }

  public class CharacterProxy : EntityProxy, ICharacter
  {
    public bool IsDead => character.IsDead;
    public bool Removed => character.Removed;
    public bool GodMode => character.GodMode;
    public string Name => character.Name;
    public bool NeedsOxygen => character.NeedsOxygen;
    public float EmpVulnerability => character.EmpVulnerability;
    public Identifier SpeciesName => character.SpeciesName;
    public ICharacterParams Params { get; }
    public ICharacterHealth CharacterHealth { get; }
    public IAnimController AnimController { get; }

    public void ApplyAfflictionToMainLimb(Affliction affliction)
    {
      character.CharacterHealth.ApplyAffliction(
        character.AnimController?.MainLimb,
        affliction
      );
    }

    private Character character;
    public CharacterProxy(Character character) : base(character)
    {
      this.character = character;
      Params = new CharacterParamsProxy(character.Params);
      CharacterHealth = new CharacterHealthProxy(character.CharacterHealth);
      AnimController = new AnimControllerProxy(character.AnimController);
    }
  }

}