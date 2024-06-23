using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    // baboon patch to fix this https://github.com/FakeFishGames/Barotrauma/discussions/14150
    public static bool ColorExtensions_Multiply_Prefix(Color color, ref float value, bool onlyAlpha, Color __result)
    {
      value = Math.Max(0, value);
      return true;
    }
  }
}