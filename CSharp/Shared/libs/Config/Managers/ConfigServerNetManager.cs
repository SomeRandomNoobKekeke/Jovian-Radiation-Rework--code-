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
  public class ConfigServerNetManager
  {
    public IConfig Config;
    public ConfigServerNetManager(IConfig config) => Config = config;

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
      Config.Facades.NetFacade.ListenForClients(Config.NetHeader + "_ask", Give);
      Config.Facades.NetFacade.ListenForClients(Config.NetHeader + "_sync", Receive);
      Config.Facades.NetFacade.ServerEncondeAndBroadcast(Config.NetHeader + "_sync", Config);
    }

    //TODO how to not fail silently here?
    public void Give(IReadMessage msg, Client client)
    {
      if (!Enabled) return;
      if (Config is null) return;
      Config.Facades.NetFacade.ServerEncondeAndSend(Config.NetHeader + "_sync", Config, client);
    }

    public void Receive(IReadMessage msg, Client client)
    {
      if (!Enabled) return;
      if (Config is null) return;
      if (!Config.Facades.NetFacade.DoesClientHasPermissions(client)) return;

      Config.NetDecode(msg);
      Config.Facades.NetFacade.ServerEncondeAndBroadcast(Config.NetHeader + "_sync", Config);
    }
  }


}