using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {

    [HarmonyPatch(typeof(LuaGame))]
    public class LuaGamePatch
    {
      [HarmonyPostfix]
      [HarmonyPatch("IsCustomCommandPermitted")]
      public static void PermitCommands(Identifier command, ref bool __result)
      {
        if (Mod.Instance.AddedCommands.Any(c => c.Names.Contains(command.Value))) __result = true;
        if (command.Value == "rad_serv_amount") __result = true;
      }
    }
  }
}