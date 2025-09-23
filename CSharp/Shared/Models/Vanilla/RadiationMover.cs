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
  public class VanillaRadiationMover : IRadiationMover
  {
    public void MoveRadiation(Radiation _, float steps)
    {
      if (!_.Enabled) return;
      if (steps <= 0) return;

      float increaseAmount = _.Params.RadiationStep * steps;

      if (_.Params.MaxRadiation > 0 && _.Params.MaxRadiation < _.Amount + increaseAmount)
      {
        increaseAmount = _.Params.MaxRadiation - _.Amount;
      }

      _.IncreaseRadiation(increaseAmount);
    }
  }



}