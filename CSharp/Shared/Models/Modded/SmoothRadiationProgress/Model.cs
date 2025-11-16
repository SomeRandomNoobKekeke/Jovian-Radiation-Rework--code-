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
  public partial class SmoothRadiationProgressModel : RadiationModel
  {
    public partial class ModelSettings : IConfig
    {
      public float TargetSpeedPercentageAtTheEndOfTheMap { get; set; } = 0.5f;

      // This takes presidence over Vanilla.StartingRadiation 
      public float StartingRadiation { get; set; } = -100.0f;
      public float RadiationSpeed { get; set; } = 60.0f;
      public float MaxTimeOnRound { get; set; } = 60.0f;
      public float MinTimeOnRound { get; set; } = 0.5f;

      public float OutpostTimeMultiplier { get; set; } = 0.25f;

      public ModelSettings()
      {
        //HACK GIGACRINGE, i don't have access to the model from its settings
        this.OnPropChanged((key, value) =>
        {
          if (Mod.CurrentModel.RadiationMover is not SmoothRadiationMover) return;

          if (key == "StartingRadiation")
          {
            if (GameMain.GameSession?.Campaign?.IsFirstRound == true)
            {
              Mod.CurrentModel.RadiationMover.InitOnFirstRound();
            }
          }
        });
      }
    }

    public override bool Debug { get; set; } = true;

    public override IRadiationMover RadiationMover { get; set; }
    public override IStepsCalculator RadiationStepsCalculator { get; set; }
  }



}