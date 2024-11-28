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
    public static void Give(object[] args)
    {
      Mod.Info("Server Give");
      IReadMessage netMessage = args[0] as IReadMessage;
      Client client = args[1] as Client;

      IWriteMessage outMsg = GameMain.LuaCs.Networking.Start("jrr_sync");
      Mod.Instance.settingsManager.Encode(outMsg);

      GameMain.LuaCs.Networking.Send(outMsg, client.Connection);
    }

    public static void Receive(object[] args)
    {
      Mod.Info("Server Receive");
      IReadMessage inMsg = args[0] as IReadMessage;
      Client client = args[1] as Client;

      if (client.Connection != GameMain.Server.OwnerConnection &&
          !client.HasPermission(ClientPermissions.All)) return;

      Mod.Instance.settingsManager.Decode(inMsg);
      Mod.Instance.settingsManager.SaveTo(IOManager.SettingsFile);

      Broadcast(Mod.Instance.settingsManager.Current);
    }

    public static void Broadcast(Settings s)
    {
      Mod.Info("Server Broadcast");
      IWriteMessage outMsg = GameMain.LuaCs.Networking.Start("jrr_sync");
      SettingsManager.Encode(s, outMsg);

      GameMain.LuaCs.Networking.Send(outMsg);
    }
  }
}