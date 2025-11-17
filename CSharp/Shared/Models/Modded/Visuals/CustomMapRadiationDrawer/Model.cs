using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public partial class CustomMapRadiationDrawerModel : RadiationModel
  {
    public partial class ModelSettings : IConfig
    {
      public Color AreaColor { get; set; } = new Color(0, 16, 32, 160);
      public Color BorderColor { get; set; } = new Color(0, 127, 255, 200);
    }
  }
}