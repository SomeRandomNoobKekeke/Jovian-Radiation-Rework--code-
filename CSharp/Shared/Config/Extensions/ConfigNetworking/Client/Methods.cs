#if CLIENT
using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Barotrauma.Networking;

namespace JovianRadiationRework
{

  public static partial class ConfigNetworking
  {
    public static void Sync(object config)
    {
      IWriteMessage outMsg = GameMain.LuaCs.Networking.Start(NetHeader + "_sync");
      NetEncoder.Encode(outMsg, config);
      GameMain.LuaCs.Networking.Send(outMsg);
    }

    public static void Ask()
    {
      IWriteMessage outMsg = GameMain.LuaCs.Networking.Start(NetHeader + "_ask");
      GameMain.LuaCs.Networking.Send(outMsg);
    }

    public static void Receive(object[] args)
    {
      IReadMessage inMsg = args[0] as IReadMessage;
      Client client = args[1] as Client;
      NetEncoder.Decode(inMsg, CurrentConfig);
    }
  }
}
#endif