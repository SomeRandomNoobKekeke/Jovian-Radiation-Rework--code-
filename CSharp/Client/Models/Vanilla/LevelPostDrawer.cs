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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JovianRadiationRework
{
  public partial class VanillaRadiationModel
  {
    public class VanillaLevelPostDrawer : ILevelPostDrawer
    {
      public void Draw(Level _, GraphicsDevice graphics, SpriteBatch spriteBatch, Camera cam)
      {
        // nothing
      }
    }
  }
}