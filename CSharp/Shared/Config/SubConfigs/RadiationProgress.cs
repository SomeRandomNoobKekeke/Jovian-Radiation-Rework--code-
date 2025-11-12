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
  public class RadiationProgress : IConfig
  {
    public SmoothLocationTransformerModel.ModelSettings LocationsTransform { get; set; }
    public SmoothRadiationProgressModel.ModelSettings RadiationMoving { get; set; }
    public AdvanceOnSaveAndQuitModel.ModelSettings SaveAndQuitAction { get; set; }
  }
}