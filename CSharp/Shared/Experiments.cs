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
  public partial class Mod : IAssemblyPlugin
  {
    public void Experiment()
    {
      Mod.Log($"{Utils.ModHookId}.AutoComplete");
    }
  }
}