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
  public static class RadiationDebug
  {
    public static DebugGate<string, object> bruh = new() { State = true };
  }
}