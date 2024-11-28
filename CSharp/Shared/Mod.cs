using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using System.Runtime.CompilerServices;
[assembly: IgnoresAccessChecksTo("Barotrauma")]
[assembly: IgnoresAccessChecksTo("DedicatedServer")]
[assembly: IgnoresAccessChecksTo("BarotraumaCore")]

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static string ModName = "Jovian Radiation Rework";
    public static Mod Instance;
    public string ModDir = "";
    public string ModVersion = "0.0.0";
    public bool Debug { get; set; }
    public SettingsManager settingsManager = new SettingsManager();

    public Harmony harmony;

    public void Initialize()
    {
      Instance = this;

      FindModFolder();
      if (ModDir.Contains("LocalMods"))
      {
        Debug = true;
        Info($"Found {ModName} in LocalMods, debug: {Debug}\n");
      }

      harmony = new Harmony("jovian.radiation.rework");
      harmony.PatchAll();

      if (Debug || GameMain.GameSession?.IsRunning == true)
      {
        Init();
      }

      AddCommands();

      MemoryUsage("Initialize");
    }

    public void Init()
    {
      settingsManager.Reset();
      settingsManager.SetProp("huhu", "bebe");
      settingsManager.SaveTo(Path.Combine(Mod.Instance.ModDir, "Settings.xml"));
      settingsManager.LoadFrom(Path.Combine(Mod.Instance.ModDir, "Settings.xml"));
      settingsManager.Print();

      InitProjSpecific();
    }

    public void OnLoadCompleted() { }
    public void PreInitPatching() { }

    public void Dispose()
    {
      RemoveCommands();
    }
  }
}