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

      Settings s = new Settings();
      //Log(Traverse.Create(s).Property("Vanilla").Property("RadiationSpeed").GetValue());
      FlatView f = new FlatView(typeof(Settings));

      f.Set(s, "Vanilla.RadiationSpeed", 2.0f);

      foreach (string d in f.Props.Keys)
      {
        Log($"{d} {f.Get(s, d)}");
      }

      InitProjSpecific();
    }

    public void OnLoadCompleted() { }
    public void PreInitPatching() { }

    public void Dispose()
    {

    }
  }
}