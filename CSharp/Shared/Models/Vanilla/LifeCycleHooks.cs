using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

using Barotrauma.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Voronoi2;


namespace JovianRadiationRework
{
  public partial class VanillaRadiationModel
  {
    public class VanillaLifeCycleHooks : ILifeCycleHooks
    {
      public VanillaRadiationModel Model { get; set; }

      public void OnLoad()
      {
        Model.DebugLog("LifeCycleHook OnLoad");
        Mod.CurrentModel.ElectronicsDamager?.ScanItems();
        Mod.CurrentModel.MetadataSetter?.SetMetadata();
        Mod.CurrentModel.HullUpgrades?.UpdateMainSubProtection();

        if (GameMain.GameSession?.Campaign?.IsFirstRound == true)
        {
          OnFirstRound();
        }
      }

      public void OnRoundStart()
      {
        Model.DebugLog("LifeCycleHook OnRoundStart");
        Mod.CurrentModel.ElectronicsDamager?.ScanItems();
        Mod.CurrentModel.HullUpgrades?.UpdateMainSubProtection();

        if (GameMain.GameSession?.Campaign?.IsFirstRound == true)
        {
          OnFirstRound();
        }
      }

      public void OnFirstRound()
      {
        Model.DebugLog("LifeCycleHook OnFirstRound");

        if (GameMain.GameSession.Map.Radiation?.Enabled == true)
        {
          Mod.CurrentModel.RadiationMover.InitOnFirstRound();
        }
      }
      public void OnRoundEnd()
      {
        Model.DebugLog("LifeCycleHook OnRoundEnd");
        Mod.CurrentModel.ElectronicsDamager?.ForgetItems();
      }
      public void OnSaveAndQuit()
      {
        Model.DebugLog("LifeCycleHook OnSaveAndQuit");
        Mod.CurrentModel.SaveAndQuitHandler?.HandleSaveAndQuit(
          GameMain.GameSession.Campaign
        );
      }
      public void OnQuit()
      {
        Model.DebugLog("LifeCycleHook OnQuit");
      }
    }



  }
}