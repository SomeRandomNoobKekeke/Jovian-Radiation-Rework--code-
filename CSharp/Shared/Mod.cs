using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

using System.Runtime.CompilerServices;
[assembly: IgnoresAccessChecksTo("Barotrauma")]
[assembly: IgnoresAccessChecksTo("DedicatedServer")]
[assembly: IgnoresAccessChecksTo("BarotraumaCore")]

namespace JovianRadiationRework
{
  public partial class Plugin : IAssemblyPlugin
  {
    public static Plugin Mod;

    public Harmony harmony;

    public void Initialize()
    {
      Mod = this;
    }

    public void OnLoadCompleted() { }
    public void PreInitPatching() { }

    public void Dispose()
    {

    }
  }
}