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
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Voronoi2;


namespace JovianRadiationRework
{
  public partial class VanillaRadiationModel
  {
    public class VanillaMapRadiationTooltip : IMapRadiationTooltip
    {
      public string GetText(Radiation _)
      {
        if (_.radiationMultiplier is int multiplier)
        {
          // This multiplier is visible only in english, kek
          return TextManager.GetWithVariable("RadiationTooltip", "[jovianmultiplier]", multiplier.ToString()).ToString();
        }

        return "";
      }
    }
  }
}