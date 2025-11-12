using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using BaroJunk;

namespace JovianRadiationRework
{
  public class Visuals : IConfig
  {
    public AmbientLightModel.ModelSettings AmbientLight { get; set; }
  }
}