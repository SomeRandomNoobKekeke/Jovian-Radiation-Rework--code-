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
  public partial class VanillaRadiationModel : RadiationModel
  {
    public override IMapRadiationDrawer MapRadiationDrawer { get; set; }
    public override IMapRadiationTooltip MapRadiationTooltip { get; set; }
  }
}