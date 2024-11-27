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

    // TODO rethink   
    public override bool Execute()
    {
      bool ok = true;

      ok |= Mod.Assert(Object.Equals(flatView.Get(settings, "bebebe"), 10), "Failed to get direct value");

      return ok;
    }

    public override void Prepare()
    {
      flatView = new FlatView(typeof(TestSettings));
      settings = new TestSettings();
    }

  }
}