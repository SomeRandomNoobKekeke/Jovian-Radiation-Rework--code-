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
  public class ConfigSettings
  {
    public IConfig Config;
    public ConfigSettings(IConfig config) => Config = config;

    public bool AutoSave
    {
      get => Config.Manager.AutoSaver.Enabled;
      set => Config.Manager.AutoSaver.Enabled = value;
    }

    public bool NetSync
    {
      get => Config.Manager.NetSync;
      set => Config.Manager.NetSync = value;
    }

    public string CommandName
    {
      get => Config.Manager.CommandsManager.CommandName;
      set => Config.Manager.CommandsManager.CommandName = value;
    }

    public bool ShouldSaveInMultiplayer { get; set; } = false;
    public bool SaveOnQuit { get; set; } = true;
    public bool SaveEveryRound { get; set; } = true;
    public string SavePath { get; set; } = null;
  }
}