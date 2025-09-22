using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;

namespace BaroJunk
{
  public class ConfigAutoSaver
  {
    public static string DefaultSavePathFor(IConfig config)
      => Path.Combine("ModSettings", "Configs", $"{config.ID}.xml");

    public bool ShouldSave =>
      GameMain.IsSingleplayer ||
      LuaCsSetup.IsServer ||
      LuaCsSetup.IsClient && Config.Settings.ShouldSaveInMultiplayer;

    private bool enabled; public bool Enabled
    {
      get => enabled;
      set
      {
        enabled = value;
        if (enabled) Initialize(); else Deactivate();
      }
    }

    public IConfig Config;
    public ConfigAutoSaver(IConfig config) => Config = config;

    private void Initialize()
    {
      Config.Settings.SavePath ??= DefaultSavePathFor(Config);
      Config.LoadSave(Config.Settings.SavePath);

      Config.Facades.HooksFacade.AddHook("stop", $"save {Config.ID} config on quit", (object[] args) =>
      {
        if (Config.Settings.SaveOnQuit && ShouldSave) Config?.Save(Config.Settings.SavePath);
        return null;
      });

      Config.Facades.HooksFacade.AddHook("roundEnd", $"save {Config.ID} config on round end", (object[] args) =>
      {
        if (Config.Settings.SaveEveryRound && ShouldSave) Config?.Save(Config.Settings.SavePath);
        return null;
      });
    }

    private void Deactivate()
    {
      Config.Facades.HooksFacade.AddHook("stop", $"save {Config.ID} config on quit", (object[] args) => null);
      Config.Facades.HooksFacade.AddHook("roundEnd", $"save {Config.ID} config on round end", (object[] args) => null);
    }


  }
}