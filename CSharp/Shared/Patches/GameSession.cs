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

    [HarmonyPatch(typeof(GameSession))]
    public class GameSessionPatch
    {
      [HarmonyPostfix]
      [HarmonyPatch("StartRound", new Type[]{
            typeof(LevelData),
            typeof(bool),
            typeof(SubmarineInfo),
            typeof(SubmarineInfo),
          }
        )
      ]
      public static void GameSession_StartRound_Postfix()
      {
        Mod.Instance.Init();
      }
    }


  }
}