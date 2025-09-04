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
    public float GetAfflictionStrengthByIdentifier(Identifier afflictionIdentifier, bool allowLimbAfflictions = true);
  }

  public class CharacterHealthProxy : ICharacterHealth
  {


    public float GetAfflictionStrengthByIdentifier(Identifier afflictionIdentifier, bool allowLimbAfflictions = true)
      => characterHealth.GetAfflictionStrengthByIdentifier(afflictionIdentifier, allowLimbAfflictions);


    private CharacterHealth characterHealth;
    public CharacterHealthProxy(CharacterHealth characterHealth)
    {
      this.characterHealth = characterHealth;
    }
  }

}