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

namespace BaroJunk_Config
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