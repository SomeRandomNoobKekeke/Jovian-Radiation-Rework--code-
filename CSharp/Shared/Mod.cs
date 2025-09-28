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
    public static Logger Logger = new Logger()
    {
      PrintFilePath = false,
    };


    public static ModelManager ModelManager = new ModelManager();
    // public static ModelManager ModelManager = new ModelManager(new RadiationModel[]{
    //   new VanillaRadiationModel(),
    //   new AmbientLightModel(),
    //   new DepthBasedDamageModel(),
    //   new ProgressiveMonsterSpawningModel(),
    //   new SmoothCharacterDamager(),
    //   new SmoothLocationTransformerModel(),
    //   new SmoothRadiationProgressModel(),
    //   new DamageToElectronicsModel(),
    // });

    private static MainConfig config; public static MainConfig Config
    {
      get
      {
        config ??= new MainConfig();
        return config;
      }
    }

    public static RadiationModel CurrentModel => ModelManager.Current;
    public void Initialize()
    {

      UTestCommands.AddCommands();
      AddCommands();
      PatchAll();
      SetupHooks();

      Experiment();

      ModelManager.ScanModels();

      Config.Settings().CommandName = "rad";

      Config.EnabledModels.ModelToggled += (name, state) =>
      {
        ModelManager.SetModelState(name, state);
      };
      config.OnConfigUpdated(() => ModelManager.SyncModelStates(Config.EnabledModels));

      Config.Settings().AutoSave = true;
      Config.Settings().NetSync = true;

      Logger.Log($"{ModInfo.AssemblyName} compiled");
      Logger.Log(CurrentModel);
    }

    public void PatchAll()
    {
      MapPatch.PatchSharedMap(Harmony);
      RadiationPatch.PatchSharedRadiation(Harmony);
      MonsterEventPatch.PatchSharedMonsterEvent(Harmony);
      LocationPatch.PatchSharedLocation(Harmony);
      CampaignModePatch.PatchSharedCampaignMode(Harmony);
      PatchProjSpecific();
    }
    public partial void PatchProjSpecific();

    public void SetupHooks()
    {
      GameMain.LuaCs.Hook.Add("roundStart", "JRR", (object[] args) =>
      {
        Mod.CurrentModel.LifeCycleHooks.OnRoundStart();
        return null;
      });

      GameMain.LuaCs.Hook.Add("roundEnd", "JRR", (object[] args) =>
      {
        Mod.CurrentModel.LifeCycleHooks.OnRoundEnd();
        return null;
      });

      GameMain.LuaCs.Hook.Add("roundStart", "JRR", (object[] args) =>
      {
        Mod.CurrentModel.LifeCycleHooks.OnRoundStart();
        return null;
      });


      GameMain.LuaCs.Hook.Add("loaded", "JRR", (object[] args) =>
      {
        Mod.CurrentModel.LifeCycleHooks.OnLoad();
        return null;
      });
    }


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