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
  public partial class AdvanceOnSaveAndQuitModel
  {
    public class ModdedSaveAndQuitHandler : ISaveAndQuitHandler
    {
      public ModelSettings Settings { get; set; }

      public AdvanceOnSaveAndQuitModel Model { get; set; }

      public void HandleSaveAndQuit(CampaignMode _)
      {
        if (!Settings.ProgressOnSaveLoad) return;
        if (GameMain.GameSession == null) return;

        float roundDuration = GameMain.GameSession.RoundDuration;

        float radSteps = Mod.CurrentModel.RadiationStepsCalculator.CalculateSteps(
          CampaignMode.TransitionType.LeaveLocation, // HACK
          roundDuration
        );

        Model.DebugLog($"roundDuration: [{roundDuration}] radSteps: [{radSteps}]");

        GameMain.GameSession.Map.Radiation?.OnStep(radSteps);
      }

    }
  }
}