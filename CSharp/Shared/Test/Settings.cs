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
      Settings s1 = manager.JustLoad(preset1Path);
      Settings s2 = manager.JustLoad(preset2Path);

      manager.Use(s1);
      manager.SaveTo(settingsPath);
      Settings s = manager.JustLoad(settingsPath);

      Describe("s1 == s1", () =>
      {
        Expect(SettingsManager.Compare(s1, s1)).ToBeEqual(true);
      });

      Describe("s1 != s2", () =>
      {
        Expect(SettingsManager.Compare(s1, s2)).ToBeEqual(false);
      });

      Describe("s1 and s2 differ in name and CriticalRadiationThreshold", () =>
      {
        List<string> diff = SettingsManager.Difference(s1, s2);

        Expect(diff.SequenceEqual(new string[] { "Name", "Vanilla.CriticalRadiationThreshold" })).ToBeEqual(true);
      });

      Describe("s == s1", () =>
      {
        Expect(SettingsManager.Compare(s1, s)).ToBeEqual(true);
      });
    }

    public override void Prepare()
    {
      manager.Reset();
      manager.SaveTo(settingsPath);

      manager.Reset();
      manager.Current.Name = "Guh1";
      manager.Current.Vanilla.CriticalRadiationThreshold = 1001;
      manager.SaveTo(preset1Path);

      manager.Reset();
      manager.Current.Name = "Guh2";
      manager.Current.Vanilla.CriticalRadiationThreshold = 1002;
      manager.SaveTo(preset2Path);
    }
  }



}