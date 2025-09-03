using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

using Barotrauma.Abilities;
using Barotrauma.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml.Linq;

namespace JovianRadiationRework
{
  public interface IRadiationParams
  {
    public float RadiationDamageAmount { get; }
    public float RadiationDamageDelay { get; }
    public float RadiationEffectMultipliedPerPixelDistance { get; }


  }

  public class RadiationParamsProxy : IRadiationParams
  {
    public float RadiationDamageAmount => radiationParams.RadiationDamageAmount;
    public float RadiationDamageDelay => radiationParams.RadiationDamageDelay;
    public float RadiationEffectMultipliedPerPixelDistance => radiationParams.RadiationEffectMultipliedPerPixelDistance;

    private RadiationParams radiationParams;
    public RadiationParamsProxy(RadiationParams radiationParams) => this.radiationParams = radiationParams;
  }

}