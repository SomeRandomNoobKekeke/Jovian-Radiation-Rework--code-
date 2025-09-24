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
  public partial class RadiationModel
  {
    public virtual ILevelPostDrawer LevelPostDrawer { get; set; }
  }
}