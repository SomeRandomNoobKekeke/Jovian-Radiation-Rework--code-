using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace JovianRadiationRework
{
  public static partial class ConfigNetworking
  {
    public static string NetHeader => ConfigManager.ConfigID ?? Utils.ModHookId;

    public static void Init()
    {
      if (!GameMain.IsMultiplayer) return;

#if CLIENT
      GameMain.LuaCs.Networking.Receive(NetHeader + "_sync", Receive);
#elif SERVER
      GameMain.LuaCs.Networking.Receive(NetHeader + "_ask", Give);
      GameMain.LuaCs.Networking.Receive(NetHeader + "_sync", Receive);
#endif

      if (ConfigManager.AutoSetup) NetSync();
    }

    public static void NetSync()
    {
#if CLIENT
      Ask();
#elif SERVER
      Broadcast();
#endif
    }
  }
}