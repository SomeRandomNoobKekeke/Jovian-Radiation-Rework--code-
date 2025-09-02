using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO;

using Barotrauma;


namespace JovianRadiationRework
{
  public class DebugTest : UTestPack
  {
    /// <summary>
    /// this can be static, i made this instance just to be sure that tests don't interfere with each other
    /// </summary>
    public class DebugContext
    {
      public DebugSwitch DebugRadiationDamage = new();

      public DebugGate<string, float> CharacterDamaged = new();
      // {
      //   Switch = DebugRadiationDamage,
      // };

      public DebugAggregator<float> DamageDealt = new();
      // {
      //   Install = (self) =>
      //   {
      //     CharacterDamaged.OnCapture += (name, damage) => self.Capture(damage);
      //   }
      // };

      public DebugContext()
      {
        CharacterDamaged.Switch = DebugRadiationDamage;
        DamageDealt.Install = (self) =>
        {
          CharacterDamaged.OnCapture += (name, damage) => self.Capture(damage);
        };
      }
    }

    public void CreateSwitchTests()
    {
      DebugContext context = new DebugContext();

      Tests.Add(new UTest(context.DebugRadiationDamage.State, false));
      context.DebugRadiationDamage.On();
      Tests.Add(new UTest(context.DebugRadiationDamage.State, true));
      context.DebugRadiationDamage.Off();
      Tests.Add(new UTest(context.DebugRadiationDamage.State, false));


      context.CharacterDamaged.Switch.On();
      Tests.Add(new UTest(context.DebugRadiationDamage.State, true));
      Tests.Add(new UTest(context.CharacterDamaged.Switch.State, true));
      context.CharacterDamaged.Switch.Off();
      Tests.Add(new UTest(context.DebugRadiationDamage.State, false));
      Tests.Add(new UTest(context.CharacterDamaged.Switch.State, false));

      Tests.Add(new UTest(context.DamageDealt.Switch.State, true));
      context.DamageDealt.Switch.Off();
      Tests.Add(new UTest(context.DamageDealt.Switch.State, false));
      context.DamageDealt.Switch.On();
      Tests.Add(new UTest(context.DamageDealt.Switch.State, true));
    }

    public void CreateDebugGateTests()
    {
      DebugContext context = new DebugContext();
      float lastDamage = 0;

      context.CharacterDamaged.OnCapture += (name, damage) => lastDamage = damage;
      context.CharacterDamaged.Capture("artie", 100);

      Tests.Add(new UTest(lastDamage, 0.0f, "Debug gate should be closed"));
      context.DebugRadiationDamage.On();
      context.CharacterDamaged.Capture("artie", 100);
      Tests.Add(new UTest(lastDamage, 100.0f, "Now should capture"));
    }

    public void CreateDebugAggregatorTests()
    {
      DebugContext context = new DebugContext();
      float lastDamage = 0;

      context.DamageDealt.OnCapture += (damage) => lastDamage = damage;

      context.CharacterDamaged.Capture("artie", 100);
      Tests.Add(new UTest(lastDamage, 0.0f, "Debug gate should be closed"));

      context.DebugRadiationDamage.On();
      context.DamageDealt.Switch.Off();
      context.CharacterDamaged.Capture("artie", 100);
      Tests.Add(new UTest(lastDamage, 0.0f, "DebugAggregator is closed"));

      context.DamageDealt.Switch.On();
      context.CharacterDamaged.Capture("artie", 100);
      Tests.Add(new UTest(lastDamage, 100.0f, "Now should capture"));
    }

    public void CreateConditionTests()
    {
      DebugContext context = new DebugContext();
      float lastDamage = 0;

      context.DamageDealt.OnCapture += (damage) => lastDamage = damage;
      context.DamageDealt.Condition = (damage) => damage > 50.0f;
      context.DebugRadiationDamage.On();
      // context.DamageDealt.Switch.On(); //this should be on by def

      context.CharacterDamaged.Capture("artie", 10.0f);
      Tests.Add(new UTest(lastDamage, 0.0f, "shouldn't capture, damage is too low"));

      context.CharacterDamaged.Capture("artie", 100.0f);
      Tests.Add(new UTest(lastDamage, 100.0f, "should capture, condition is satisfied"));
    }
  }
}