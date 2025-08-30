
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
#if CLIENT
    public static bool HasPermissions => GameMain.IsSingleplayer || GameMain.Client?.IsServerOwner == true || GameMain.Client?.HasPermission(ClientPermissions.ConsoleCommands) == true;

    public static bool Sync()
    {
      if(GameMain.IsSingleplayer) return false;
      if(!HasPermissions){
        Mod.Warning($"You need to be host or have ConsoleCommands permission to use it");
        return false;
      }

      IWriteMessage outMsg = GameMain.LuaCs.Networking.Start(NetHeader + "_sync");
      NetEncoder.Encode(outMsg, ConfigManager.CurrentConfig);
      GameMain.LuaCs.Networking.Send(outMsg);

      return true;
    }

    public static void Ask()
    {
      IWriteMessage outMsg = GameMain.LuaCs.Networking.Start(NetHeader + "_ask");
      GameMain.LuaCs.Networking.Send(outMsg);
    }

    public static void Receive(object[] args)
    {
      IReadMessage inMsg = args[0] as IReadMessage;
      NetEncoder.Decode(inMsg, ConfigManager.CurrentConfig);
    }
#endif

#if SERVER
    public static bool HasPermissions(Client client)
      => client.Connection == GameMain.Server.OwnerConnection || client.HasPermission(ClientPermissions.ConsoleCommands);

    public static void Give(object[] args)
    {
      IReadMessage netMessage = args[0] as IReadMessage;
      Client client = args[1] as Client;

      IWriteMessage outMsg = GameMain.LuaCs.Networking.Start(NetHeader + "_sync");
      NetEncoder.Encode(outMsg, ConfigManager.CurrentConfig);
      GameMain.LuaCs.Networking.Send(outMsg, client.Connection);
    }

    public static void Receive(object[] args)
    {
      IReadMessage inMsg = args[0] as IReadMessage;
      Client client = args[1] as Client;

      if(!HasPermissions(client)){
        Mod.Log($"{client} tried to sync settings, but he doesn't have permissions");
        return;
      }

      NetEncoder.Decode(inMsg, ConfigManager.CurrentConfig);
      Broadcast();
    }

    public static void Broadcast()
    {
      IWriteMessage outMsg = GameMain.LuaCs.Networking.Start(NetHeader + "_sync");
      NetEncoder.Encode(outMsg, ConfigManager.CurrentConfig);
      GameMain.LuaCs.Networking.Send(outMsg);
    }
#endif

  }
}
