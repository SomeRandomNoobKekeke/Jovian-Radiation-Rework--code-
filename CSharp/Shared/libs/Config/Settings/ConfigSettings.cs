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
    public ConfigCore Config;
    public ConfigSettings(ConfigCore config) => Config = config;

    public string CommandName
    {
      get => Config.Manager.CommandsManager.CommandName;
      set => Config.Manager.CommandsManager.CommandName = value;
    }

    /// <summary>
    /// Deep reactivity wasn't planned and is very unoptimized, so it's optional and defaults to false
    /// </summary>
    public bool DeeplyReactive
    {
      get => Config.ReactiveCore.DeeplyReactive;
      set => Config.ReactiveCore.DeeplyReactive = value;
    }

    public ConfigStrategy Strategy
    {
      get => strategy;
      set
      {
        strategy = value;
        Config.Manager.UseStrategy(value);
      }
    }
    private ConfigStrategy strategy;

    public bool SyncOnPropChanged { get; set; } = true;
    public string SavePath { get; set; } = null;
    public bool PrintAsXML { get; set; } = false;
  }
}