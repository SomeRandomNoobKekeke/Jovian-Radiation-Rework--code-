using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using BaroJunk;
using BaroJunk_Config;


namespace JovianRadiationRework
{
  public class VanillaSettings : RadiationParamsFacade, IConfig
  {
    public event Action<float> StartingRadiationSet;
    public void RaiseStartingRadiationSet(float amount) => StartingRadiationSet?.Invoke(amount);
  }
}