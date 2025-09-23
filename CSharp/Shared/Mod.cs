using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using BaroJunk;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static Harmony Harmony = new Harmony("JovianRadiationRework");
    public static Logger Logger = new Logger();

    public static LogicContainer LogicContainer = new LogicContainer();

    public void Initialize()
    {

      UTestCommands.AddCommands();
      PatchAll();

      // UTestPack.RunRecursive<DebugTest>();

      Experiment();
      Logger.Log($"{ModInfo.AssemblyName} compiled");
    }

    public void PatchAll()
    {
      MapPatch.PatchSharedMap(Harmony);
      RadiationPatch.PatchSharedRadiation(Harmony);
    }
    public void OnLoadCompleted() { }
    public void PreInitPatching() { }
    public void Dispose()
    {
      Harmony.UnpatchSelf();
      UTestCommands.RemoveCommands();
    }
  }
}