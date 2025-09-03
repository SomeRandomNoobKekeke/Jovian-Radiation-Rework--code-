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
  public interface IHealthParams
  {
    public bool StunImmunity { get; }
    public bool PoisonImmunity { get; }
    public IEnumerable<Identifier> ImmunityIdentifiers { get; }
  }

  public class HealthParamsProxy : IHealthParams
  {
    public bool StunImmunity => healthParams.StunImmunity;
    public bool PoisonImmunity => healthParams.PoisonImmunity;
    public IEnumerable<Identifier> ImmunityIdentifiers => healthParams.ImmunityIdentifiers;

    private CharacterParams.HealthParams healthParams;
    public HealthParamsProxy(CharacterParams.HealthParams healthParams) => this.healthParams = healthParams;
  }

}