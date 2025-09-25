using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

namespace JovianRadiationRework
{
  public class AfflictionWrapper
  {
    public AfflictionPrefab AfflictionPrefab { get; set; }

    public AfflictionWrapper(AfflictionPrefab afflictionPrefab) => AfflictionPrefab = afflictionPrefab;
  }



}