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
  public partial class SmoothLocationTransformerModel
  {
    public class SmoothLocationIsCriticallyRadiated : ILocationIsCriticallyRadiated
    {
      public ModelSettings Settings { get; set; }

      public bool IsIt(Location _)
      {
        if (GameMain.GameSession?.Map?.Radiation != null)
        {
          return GameMain.GameSession.Map.Radiation.Amount - _.MapPosition.X >
            Settings.CriticalOutpostRadiationAmount;
        }

        return false;
      }
    }
  }




}