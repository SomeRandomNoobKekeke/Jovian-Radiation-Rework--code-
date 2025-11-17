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
  public partial class CustomTooltipModel
  {
    public class CustomMapRadiationTooltip : IMapRadiationTooltip
    {
      public string GetText(Radiation _)
      {
        return TextManager.Get("RadiationTooltip").ToString()
                          .Replace("(LEVEL [jovianmultiplier])", "")
                          .Replace("\n", $" ({_.radiationMultiplier} JV/S)\n");
      }
    }
  }
}