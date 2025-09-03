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
  public interface ILimb
  {
    public LimbType type { get; }
    public int HealthIndex { get; }
  }

  public class LimbProxy : ILimb
  {
    public LimbType type => limb.type;
    public int HealthIndex => limb.HealthIndex;

    private Limb limb { get; }
    public LimbProxy(Limb limb) => this.limb = limb;
  }

}