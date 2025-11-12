using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JovianRadiationRework
{
  public partial interface ILevelPostDrawer : IModelAspect
  {
    public void Draw(Level _, GraphicsDevice graphics, SpriteBatch spriteBatch, Camera cam);
  }

  public partial interface IMapRadiationDrawer : IModelAspect
  {
    public void Draw(Radiation _, SpriteBatch spriteBatch, Rectangle container, float zoom);
  }

  public partial interface IMapRadiationTooltip : IModelAspect
  {
    public string GetText(Radiation _);
  }
}