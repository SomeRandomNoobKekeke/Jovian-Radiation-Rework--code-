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
  public class VanillaRadAmountCalculator : IRadAmountCalculator
  {
    public float CalculateAmount(Radiation _, Entity entity)
    {
      return _.DepthInRadiation(entity);
    }
  }



}