using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Barotrauma.Extensions;
using Barotrauma.Networking;
namespace JovianRadiationRework
{
  public partial class Mod
  {
    public partial struct Settings
    {
      public static void net_recieve_init(object[] args)
      {
        info("net_recieve_init server");
        IReadMessage netMessage = args[0] as IReadMessage;
        Client client = args[1] as Client;

        IWriteMessage message = GameMain.LuaCs.Networking.Start("jrr_init");
        Settings.encode(settings, message);
        settings.print();

        GameMain.LuaCs.Networking.Send(message);
      }

      public static void net_recieve_sync(object[] args)
      {
        info("net_recieve_sync server");

        IReadMessage inMsg = args[0] as IReadMessage;
        Client client = args[1] as Client;

        if (client.Connection != GameMain.Server.OwnerConnection &&
            !client.HasPermission(ClientPermissions.All)) return;


        Settings.decode(settings, inMsg);

        IWriteMessage outMsg = GameMain.LuaCs.Networking.Start("jrr_sync");
        Settings.encode(settings, outMsg);

        GameMain.LuaCs.Networking.Send(outMsg);
      }
    }

  }
}