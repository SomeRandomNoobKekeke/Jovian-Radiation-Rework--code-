using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Barotrauma.Extensions;
using Barotrauma.Items.Components;
using FarseerPhysics;

namespace JovianRadiationRework
{

  public partial class MonsterEventPatch
  {
    public static void PatchSharedMonsterEvent(Harmony harmony)
    {
      harmony.Patch(
        original: typeof(MonsterEvent).GetMethod("InitEventSpecific", AccessTools.all),
        prefix: new HarmonyMethod(typeof(MonsterEventPatch).GetMethod("MonsterEvent_InitEventSpecific_Replace"))
      );
    }

    public static bool MonsterEvent_InitEventSpecific_Replace(EventSet parentSet, MonsterEvent __instance)
    {
      MonsterEvent _ = __instance;

      // apply pvp stun resistance (reduce stun amount via resist multiplier)
      if (GameMain.NetworkMember is { } networkMember && GameMain.GameSession?.GameMode is PvPMode && !networkMember.ServerSettings.PvPSpawnMonsters)
      {
        if (GameSettings.CurrentConfig.VerboseLogging)
        {
          DebugConsole.NewMessage($"PvP setting: disabling monster event ({_.SpeciesName})", Color.Yellow);
        }

        _.disallowed = true;
        return false;
      }

      if (parentSet != null && _.resetTime == 0)
      {
        // Use the parent reset time only if there's no reset time defined for the event.
        _.resetTime = parentSet.ResetTime;
      }
      if (GameSettings.CurrentConfig.VerboseLogging)
      {
        DebugConsole.NewMessage("Initialized MonsterEvent (" + _.SpeciesName + ")", Color.White);
      }

      _.monsters.Clear();

      Mod.CurrentModel.MonsterSpawner.SpawnMonsters(_);

      return false;
    }


  }

}