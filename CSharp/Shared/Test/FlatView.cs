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
      public TestNestedSettings nest { get; set; } = new TestNestedSettings();
    }

    public class TestNestedSettings
    {
      public string jujuju { get; set; } = "kokoko";
    }

    public FlatView flatView;
    public TestSettings settings;

    public override void Execute()
    {
      Describe("Level 1", () =>
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
      });

      Describe("Level 2", () =>
      {
        Describe("get level 2 value, should be equal kokoko", () =>
        {
          Expect(flatView.Get(settings, "nest.jujuju")).ToBeEqual("kokoko");
        });

        Describe("setting it to jujuju", () =>
        {
          Expect(() => flatView.Set(settings, "nest.jujuju", "jujuju")).ToNotThrow();
        });

        Describe("now should be equal 11", () =>
        {
          Expect(flatView.Get(settings, "nest.jujuju")).ToBeEqual("jujuju"
          );
        });
      });

      Describe("The funny", () =>
      {
        Describe("access empty nested prop", () =>
        {
          Expect(() => flatView.Get(settings, "bebebe.")).ToNotThrow();
        });

        Describe("access nested prop that doesn't exist", () =>
        {
          Expect(() => flatView.Get(settings, "bebebe.kokoko")).ToNotThrow();
        });

        Describe("get null", () =>
        {
          Expect(() => flatView.Get(settings, null)).ToNotThrow();
        });

        Describe("get null on a null", () =>
        {
          Expect(() => flatView.Get(null, null)).ToNotThrow();
        });

        Describe("get something on a null", () =>
        {
          Expect(() => flatView.Get(null, "bebebe")).ToNotThrow();
        });
      });


    }

    public override void Prepare()
    {
      flatView = new FlatView(typeof(TestSettings));
      settings = new TestSettings();
    }

  }
}