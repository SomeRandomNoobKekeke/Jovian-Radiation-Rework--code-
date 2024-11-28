using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Barotrauma.Extensions;
using Barotrauma.Networking;

namespace JovianRadiationRework
{
  public partial class NetManager
  {
    public static void Sync(Settings s)
    {
      Mod.Info("Client Sync");

      if (GameMain.IsSingleplayer) return;

      IWriteMessage outMsg = GameMain.LuaCs.Networking.Start("jrr_sync");
      Mod.Instance.settingsManager.Encode(outMsg);

      GameMain.LuaCs.Networking.Send(outMsg);
    }

    public static void Ask()
    {
      Mod.Info("Client Ask");
      IWriteMessage message = GameMain.LuaCs.Networking.Start("jrr_ask");
      GameMain.LuaCs.Networking.Send(message);
    }

    public static void Receive(object[] args)
    {
      Mod.Info("Client Receive");
      IReadMessage inMsg = args[0] as IReadMessage;
      Client client = args[1] as Client;
      Mod.Instance.settingsManager.Decode(inMsg);
    }
  }
}