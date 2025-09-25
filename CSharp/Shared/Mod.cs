global using BaroJunk;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;


namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static Harmony Harmony = new Harmony("JovianRadiationRework");
    public static Logger Logger = new Logger();


    public static ModelManager ModelManager = new ModelManager(new RadiationModel[]{
      new VanillaRadiationModel(),
      new AmbientLightModel(),
      new DepthBasedDamageModel(),
      new ProgressiveMonsterSpawningModel(),
      new ProgressiveCharacterDamagerModel(),
      new SmoothLocationTransformerModel(),
      new SmoothRadiationProgressModel(),
    });

    public static MainConfig Config { get; set; } = new MainConfig();
    public static RadiationModel CurrentModel => ModelManager.Current;
    public void Initialize()
    {

      UTestCommands.AddCommands();
      AddCommands();
      PatchAll();

      // UTestPack.RunRecursive<DebugTest>();

      Experiment();
      // ModelManager.EnableModel(typeof(AmbientLightModel));

      Logger.Log($"{ModInfo.AssemblyName} compiled");
      Logger.Log(CurrentModel);
    }

    public void PatchAll()
    {
      MapPatch.PatchSharedMap(Harmony);
      RadiationPatch.PatchSharedRadiation(Harmony);
      MonsterEventPatch.PatchSharedMonsterEvent(Harmony);
      LocationPatch.PatchSharedLocation(Harmony);
      PatchProjSpecific();
    }
    public partial void PatchProjSpecific();


    public void OnLoadCompleted() { }
    public void PreInitPatching() { }
    public void Dispose()
    {
      Harmony.UnpatchSelf();
      RemoveCommands();
      UTestCommands.RemoveCommands();
    }
  }
}