using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

using System.Runtime.CompilerServices;
[assembly: IgnoresAccessChecksTo("Barotrauma")]
[assembly: IgnoresAccessChecksTo("DedicatedServer")]
[assembly: IgnoresAccessChecksTo("BarotraumaCore")]

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static string ModName = "Jovian Radiation Rework";
    public static Mod instance;
    public string ModDir = "";
    public string ModVersion = "0.0.0";
    public bool Debug { get; set; }
    public SettingsManager settingsManager;

    public Harmony harmony;

    public void Initialize()
    {
      instance = this;
      FindModFolder();
      if (ModDir.Contains("LocalMods"))
      {
        Debug = true;
        Info($"Found {ModName} in LocalMods, debug: {Debug}\n");
      }

      harmony = new Harmony("jovian.radiation.rework");


      settingsManager = new SettingsManager();
      settingsManager.Reset();



      InitProjSpecific();
    }

    public void OnLoadCompleted() { }
    public void PreInitPatching() { }

    public void Dispose()
    {

    }
  }
}