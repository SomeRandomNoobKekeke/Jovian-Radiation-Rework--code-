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

  public partial class LevelPatch
  {
    public static void PatchClientLevel(Harmony harmony)
    {
      harmony.Patch(
        original: typeof(Level).GetMethod("DrawBack", AccessTools.all),
        postfix: new HarmonyMethod(typeof(LevelPatch).GetMethod("Level_DrawBack_Postfix"))
      );
    }

    public static void Level_DrawBack_Postfix(Level __instance, GraphicsDevice graphics, SpriteBatch spriteBatch, Camera cam)
    {
      Mod.CurrentModel.LevelPostDrawer?.Draw(__instance, graphics, spriteBatch, cam);
    }

  }

}