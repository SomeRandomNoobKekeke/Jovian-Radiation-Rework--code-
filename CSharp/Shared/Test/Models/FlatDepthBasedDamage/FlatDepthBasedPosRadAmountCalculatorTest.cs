using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Text;
using Barotrauma;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public class FlatDepthBasedPosRadAmountCalculatorTest : ModelAspectTest
  {
    public override void CreateTests()
    {
      FlatDepthBasedDamageModel.FlatDepthBasedPosRadAmountCalculator calculator = new()
      {
        Settings = new FlatDepthBasedDamageModel.ModelSettings()
        {
          WaterRadiationBlockPerMeter = 1.0f,
        },
        Level_Loaded = new FakeCurrentLevelFacade()
        {
          Data = new FakeCurrentLevelData()
          {
            IsLoaded = true,
            Type = LevelData.LevelType.LocationConnection,
            StartPosition = new Vector2(0, 0),
            EndPosition = new Vector2(10000, 0),
            StartLocation_MapPosition = new Vector2(200, 0),
            EndLocation_MapPosition = new Vector2(300, 0),
          }
        },
        RadiationAccessor = new FakeRadiationAccessor()
        {
          Data = new FakeRadiationAccessorData()
          {
            Enabled = true,
            Amount = 300,
          }
        }
      };

      Tests.Add(new UTest(calculator.CalculateAmount(null, new Vector2(0, 0)), 100.0f));
      Tests.Add(new UTest(calculator.CalculateAmount(null, new Vector2(5000, 0)), 50.0f));
      Tests.Add(new UTest(calculator.CalculateAmount(null, new Vector2(10000, 0)), 0.0f));
    }
  }
}