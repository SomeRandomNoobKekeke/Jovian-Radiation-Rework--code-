using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

using Barotrauma.Networking;
using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace JovianRadiationRework
{

  public partial class LevelRendererPatch
  {

    public static void PatchClientLevelRenderer(Harmony harmony)
    {
      harmony.Patch(
        original: typeof(LevelRenderer).GetMethod("DrawDebugOverlay", AccessTools.all),
        postfix: new HarmonyMethod(typeof(LevelRendererPatch).GetMethod("LevelRenderer_DrawDebugOverlay_Postfix"))
      );
    }

    /// <summary>
    /// Made this just to test HullRadiationProtection.Cave
    /// </summary>
    public static void LevelRenderer_DrawDebugOverlay_Postfix(LevelRenderer __instance, SpriteBatch spriteBatch, Camera cam)
    {
      foreach (Level.Cave cave in Level.Loaded.Caves)
      {
        if (Character.Controlled is not null && cave.Area.Contains(Character.Controlled.WorldPosition))
        {
          GUI.DrawRectangle(spriteBatch,
            new Vector2(cave.Area.X, -cave.Area.Y - cave.Area.Height),
            new Vector2(cave.Area.Width, cave.Area.Height),
          Color.Cyan * 0.25f, true);
        }
        else
        {
          GUI.DrawRectangle(spriteBatch,
            new Vector2(cave.Area.X, -cave.Area.Y - cave.Area.Height),
            new Vector2(cave.Area.Width, cave.Area.Height),
          Color.Yellow * 0.25f, true);
        }
      }
    }


  }

}