using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;


namespace JovianRadiationRework
{
  public class FlatViewTest : Test
  {
    public class TestSettings
    {
      public int bebebe { get; set; } = 10;
    }

    public FlatView flatView;
    public TestSettings settings;

    public override void Execute()
    {
      Describe("get level 1 value, should be equal 10", () =>
      {
        Expect(flatView.Get(settings, "bebebe")).ToBeEqual(10);
      });

      Describe("setting it to 11", () =>
      {
        Expect(() => flatView.Set(settings, "bebebe", 11)).ToNotThrow();
      });

      Describe("now should be equal 11", () =>
      {
        Expect(flatView.Get(settings, "bebebe")).ToBeEqual(11);
      });
    }

    public override void Prepare()
    {
      flatView = new FlatView(typeof(TestSettings));
      settings = new TestSettings();
    }

  }
}