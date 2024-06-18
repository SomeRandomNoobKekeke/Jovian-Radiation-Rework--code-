using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Barotrauma.Extensions;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public Harmony harmony;

    public void Initialize()
    {
      harmony = new Harmony("radiation.rework");

      patchAll();
      addCommands();
      figureOutModVersionAndDirPath();
      init();

      info($"{meta.ModName} | {meta.ModVersion} - Compiled");
    }

    public static void init()
    {
      settings.vanilla.apply();
    }

    public void patchAll()
    {
      harmony.Patch(
        original: typeof(MonsterEvent).GetMethod("InitEventSpecific", AccessTools.all),
        prefix: new HarmonyMethod(typeof(Mod).GetMethod("MonsterEvent_InitEventSpecific_Replace"))
      );

      harmony.Patch(
        original: typeof(Radiation).GetMethod("UpdateRadiation", AccessTools.all),
        prefix: new HarmonyMethod(typeof(Mod).GetMethod("Radiation_UpdateRadiation_Replace"))
      );

      harmony.Patch(
        original: typeof(GameSession).GetMethod("StartRound", AccessTools.all, new Type[]{
          typeof(LevelData),
          typeof(bool),
          typeof(SubmarineInfo),
          typeof(SubmarineInfo),
        }),
        postfix: new HarmonyMethod(typeof(Mod).GetMethod("init"))
      );


      harmony.Patch(
        original: typeof(Level).GetMethod("DrawBack", AccessTools.all),
        postfix: new HarmonyMethod(typeof(Mod).GetMethod("Level_DrawBack_Postfix"))
      );
    }

    public void OnLoadCompleted() { }
    public void PreInitPatching() { }

    public void Dispose()
    {
      harmony.UnpatchAll(harmony.Id);
      harmony = null;

      removeCommands();
    }
  }


}