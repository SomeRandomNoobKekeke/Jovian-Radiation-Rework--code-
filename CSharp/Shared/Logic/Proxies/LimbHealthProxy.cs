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
  public interface ILimbHealth
  {

  }

  public class LimbHealthProxy : ILimbHealth
  {

    private CharacterHealth.LimbHealth limbHealth { get; }
    public LimbHealthProxy(CharacterHealth.LimbHealth limbHealth) => this.limbHealth = limbHealth;
  }

}