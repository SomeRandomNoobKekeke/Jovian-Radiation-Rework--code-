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
using Barotrauma.Networking;

namespace BaroJunk
{
  public class ConfigClientNetManager
  {
    public IConfig Config;
    public ConfigClientNetManager(IConfig config) => Config = config;

    private bool enabled; public bool Enabled
    {
      get => enabled;
      set
      {
        enabled = value;
        if (enabled) Initialize();
      }
    }

    private void Initialize()
    {
      if (!Config.Facades.NetFacade.IsMultiplayer) return;
      Config.Facades.NetFacade.ListenForServer(Config.NetHeader + "_sync", Receive);
      Config.Facades.NetFacade.ClientSend(Config.NetHeader + "_ask");
    }

    public void Receive(IReadMessage msg)
    {
      if (!Enabled) return;
      Config?.NetDecode(msg);
    }
  }
}