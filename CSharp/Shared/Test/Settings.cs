using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using System.IO;

namespace JovianRadiationRework
{
  public class SettingsTest : Test
  {
    SettingsManager manager = new SettingsManager();
    public string testFolder => Path.Combine(Mod.Instance.ModDir, "Ignore", "Test", "SettingsNameTest");
    public string settingsPath => Path.Combine(testFolder, "Settings.xml");
    public string preset1Path => Path.Combine(testFolder, "Preset1.xml");
    public string preset2Path => Path.Combine(testFolder, "Preset2.xml");

    public override void Execute()
    {
      Describe("Loading normal settings", () =>
      {
        manager.LoadFrom(settingsPath);
        Expect(manager.Current.Name).ToBeEqual("Settings");
      });

      Describe("Loading guh1", () =>
      {
        manager.LoadFrom(preset1Path);
        Expect(manager.Current.Name).ToBeEqual("Guh1");
      });

      Describe("Loading guh2", () =>
      {
        manager.LoadFrom(preset2Path);
        Expect(manager.Current.Name).ToBeEqual("Guh2");
      });
    }

    public override void Prepare()
    {
      manager.Reset();
      manager.SaveTo(settingsPath);

      manager.Reset();
      manager.Current.Name = "Guh1";
      manager.SaveTo(preset1Path);

      manager.Reset();
      manager.Current.Name = "Guh2";
      manager.SaveTo(preset2Path);
    }
  }



}