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
    public static Mod Instance;
    public static Harmony Harmony = new Harmony("JovianRadiationRework");
    public static Logger Logger = new Logger()
    {
      PrintFilePath = false,
    };


    public static ModelManager ModelManager = new ModelManager();

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
      Instance = this;
      UTestCommands.AddCommands();
      AddCommands();
      PatchAll();
      SetupHooks();

      Experiment();

      ModelManager.ScanModels();

      Config.Settings().SavePath = MainConfig.DefaultConfigPath;
      Config.Settings().CommandName = "rad";
      Config.Settings().DeeplyReactive = true;


      Config.EnabledModels.ModelToggled += (name, state) =>
      {
        ModelManager.SetModelState(name, state);
      };

      Config.Vanilla.StartingRadiationSet += (amount) =>
      {
        if (GameMain.GameSession?.Campaign?.IsFirstRound == true)
        {
          if (GameMain.GameSession.Map.Radiation?.Enabled == true)
          {
            GameMain.GameSession.Map.Radiation.Amount = amount;
            Logger.Warning($"Vanilla.StartingRadiation is set on first round, changing Radiation.Amount");
          }
        }
      };


      Config.OnUpdated(() => ModelManager.SyncModelStates(Config.EnabledModels));

      Config.UseStrategy(ConfigStrategy.MultiplayerClientside);

      ProjectInfo.CheckIncompatibleLibs();
      Logger.Log($"{ModInfo.AssemblyName} compiled");
      // Logger.Log(CurrentModel);
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

    public void DestroyStaticFields()
    {
      foreach (FieldInfo fi in typeof(Mod).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
      {
        if (!fi.FieldType.IsPrimitive)
        {
          fi.SetValue(this, null);
        }
      }
    }

    public void Dispose()
    {
      Harmony.UnpatchSelf();
      RemoveCommands();
      UTestCommands.RemoveCommands();
      RadiationParamsAccess.Instance.Reset();
      DestroyStaticFields();
    }
  }
}