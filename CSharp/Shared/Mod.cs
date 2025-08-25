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
  public partial class Mod : IAssemblyPlugin
  {
    public static Harmony Harmony = new Harmony("JovianRadiationRework");

    public static ConfigTest.ExampleConfigs.ConfigA Config = new();

    public void Initialize()
    {
      PatchAll();
      AddCommands();
      UTestCommands.AddCommands();

      // ConfigNetworking.Use(Config);
      // ConfigCommands.Use(Config, "be");


      UTestPack.RunRecursive<ConfigTest.NetEncoderTest>();
    }

    public void PatchAll()
    {
      BetterConsoleAutocomplete.Patch(Harmony);
    }
    public void OnLoadCompleted() { }
    public void PreInitPatching() { }
    public void Dispose()
    {
      Harmony.UnpatchSelf();
      RemoveCommands();
      UTestCommands.RemoveCommands();
    }
  }
}