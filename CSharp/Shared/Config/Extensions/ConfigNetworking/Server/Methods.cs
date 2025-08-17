#if SERVER
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
  public static class ConfigNetworking
  {
    public static void Give(object[] args)
    {
      IReadMessage netMessage = args[0] as IReadMessage;
      Client client = args[1] as Client;

      IWriteMessage outMsg = GameMain.LuaCs.Networking.Start(NetHeader + "_sync");
      NetEncoder.Encode(outMsg, CurrentConfig);
      GameMain.LuaCs.Networking.Send(outMsg, client.Connection);
    }

    public static void Receive(object[] args)
    {
      IReadMessage inMsg = args[0] as IReadMessage;
      Client client = args[1] as Client;

      NetEncoder.Decode(inMsg, CurrentConfig);
      Broadcast(CurrentConfig);
    }

    public static void Broadcast(object config)
    {
      IWriteMessage outMsg = GameMain.LuaCs.Networking.Start(NetHeader + "_sync");
      NetEncoder.Encode(outMsg, CurrentConfig);
      GameMain.LuaCs.Networking.Send(outMsg);
    }
  }
}
#endif