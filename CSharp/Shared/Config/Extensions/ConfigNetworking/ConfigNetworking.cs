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
    public static object CurrentConfig;
    public static string NetHeader;

    public static void InstallNetHooks()
    {
#if CLIENT
      GameMain.LuaCs.Networking.Receive(NetHeader + "_sync", Receive);
#elif SERVER
      GameMain.LuaCs.Networking.Receive(NetHeader + "_ask", Give);
      GameMain.LuaCs.Networking.Receive(NetHeader + "_sync", Receive);
#endif
    }

    public static void Init()
    {
#if CLIENT
      Ask();
#elif SERVER
      Broadcast();
#endif
    }

    public static void Use(object config)
    {
      CurrentConfig = config;
      NetHeader = $"{CurrentConfig.GetType().Namespace}_{CurrentConfig.GetType().Name}";

      if (!GameMain.IsMultiplayer) return;

      InstallNetHooks();
      Init();
    }

  }
}