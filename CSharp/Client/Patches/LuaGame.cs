using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public class LuaGamePatch
  {
    public static void PatchClientLuaGame(Harmony harmony)
    {
      harmony.Patch(
        original: typeof(LuaGame).GetMethod("IsCustomCommandPermitted", AccessTools.all),
        postfix: new HarmonyMethod(typeof(LuaGamePatch).GetMethod("PermitCommands"))
      );
    }
    public static void PermitCommands(Identifier command, ref bool __result)
    {
      if (Mod.Instance.AddedCommands.Any(c => c.Names.Contains(command.Value))) __result = true;
    }
  }
}