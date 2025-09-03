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
  public interface ICharacterParams
  {
    public bool IsMachine { get; }
    public IHealthParams Health { get; }
  }

  public class CharacterParamsProxy : ICharacterParams
  {
    public bool IsMachine => characterParams.IsMachine;
    public IHealthParams Health { get; }

    private CharacterParams characterParams;
    public CharacterParamsProxy(CharacterParams characterParams)
    {
      this.characterParams = characterParams;
      Health = new HealthParamsProxy(characterParams.Health);
    }
  }

}