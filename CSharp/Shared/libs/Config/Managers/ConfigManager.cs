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
  public class ConfigManager
  {
    public IConfig Config;




    public ConfigAutoSaver AutoSaver;
    public ConfigClientNetManager ClientNetController;
    public ConfigServerNetManager ServerNetController;
    public ConfigCommandsManager CommandsManager;

    //HACK (cringe)
    public bool NetSync
    {
      get
      {
        return Config.Facades.NetFacade.IsClient ? ClientNetController.Enabled : ServerNetController.Enabled;
      }

      set
      {
        if (Config.Facades.NetFacade.IsClient)
        {
          ClientNetController.Enabled = value;
        }
        else
        {
          ServerNetController.Enabled = value;
        }
      }
    }

    public ConfigManager(IConfig config)
    {
      Config = config;
      AutoSaver = new ConfigAutoSaver(config);
      ClientNetController = new ConfigClientNetManager(config);
      ServerNetController = new ConfigServerNetManager(config);
      CommandsManager = new ConfigCommandsManager(config);
    }
  }
}