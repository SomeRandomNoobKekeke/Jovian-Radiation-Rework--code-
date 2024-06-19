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
      public static void sync(Settings s)
      {
        if (GameMain.IsSingleplayer) return;

        IWriteMessage message = GameMain.LuaCs.Networking.Start("jrr_sync");
        Settings.encode(s, message);

        info("sync start");

        GameMain.LuaCs.Networking.Send(message);
      }

      public static void askServerForSettings()
      {
        info("init start");
        IWriteMessage message = GameMain.LuaCs.Networking.Start("jrr_init");
        GameMain.LuaCs.Networking.Send(message);
      }

      public static void net_recieve_init(object[] args)
      {
        info("net_recieve_init client");

        IReadMessage netMessage = args[0] as IReadMessage;
        Client client = args[1] as Client;

        try
        {
          Settings.decode(settings, netMessage);
          settings.print();
          settings.apply();
          log("Radiation settings initialised");
        }
        catch (Exception e) { err(e); }
      }

      public static void net_recieve_sync(object[] args)
      {
        info("net_recieve_sync client");

        IReadMessage netMessage = args[0] as IReadMessage;
        Client client = args[1] as Client;

        Settings.decode(settings, netMessage);
        log("Sonar markers settings changed");
      }
    }

  }
}