using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

using Barotrauma.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Voronoi2;


namespace JovianRadiationRework
{
  public partial class HuskRadiationResistanceModel
  {
    public class CustomHuskResistanceCalculator : IHuskResistanceCalculator
    {
      public ModelSettings Settings { get; set; }
      public float GetHuskResistanceMult(Radiation _, Character character)
      {
        return 1.0f;
      }
    }
  }
}