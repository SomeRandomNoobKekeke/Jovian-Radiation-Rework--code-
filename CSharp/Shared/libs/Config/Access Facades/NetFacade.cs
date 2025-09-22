using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Barotrauma.Networking;

namespace BaroJunk
{
  public interface INetFacade
  {
    public bool IsMultiplayer { get; }
    public bool IsClient { get; }
    public void ClientSend(string header);
    public void ClientEncondeAndSend(string header, IConfig config);
    public void ServerSend(string header, Client client);
    public void ServerEncondeAndSend(string header, IConfig config, Client client);
    public void ServerEncondeAndBroadcast(string header, IConfig config);
    public void ListenForServer(string header, Action<IReadMessage> callback);
    public void ListenForClients(string header, Action<IReadMessage, Client> callback);
    public bool DoesClientHasPermissions(Client client);
    public bool DoIHavePermissions();
  }

  public class NetFacade : INetFacade
  {
    //TODO think, where should it be
    public ClientPermissions RequiredPermissions = ClientPermissions.ConsoleCommands;
    public bool IsMultiplayer => GameMain.IsMultiplayer;

#if CLIENT
    public bool IsClient =>true;

    public bool DoIHavePermissions()
      => GameMain.IsSingleplayer || GameMain.Client?.IsServerOwner == true || GameMain.Client?.HasPermission(RequiredPermissions) == true;

    public void ClientSend(string header)
    {
      GameMain.LuaCs.Networking.Send(GameMain.LuaCs.Networking.Start(header));
    }

    public void ClientEncondeAndSend(string header, IConfig config)
    {
      IWriteMessage outMsg = GameMain.LuaCs.Networking.Start(header);
      config.NetEncode(outMsg);
      GameMain.LuaCs.Networking.Send(outMsg);
    }

    public void ListenForServer(string header, Action<IReadMessage> callback)
    {
      GameMain.LuaCs.Networking.Receive(header, (object[] args) =>
      {
        callback?.Invoke(args[0] as IReadMessage);
      });
    }

    public bool DoesClientHasPermissions(Client client) => false;
    public void ServerSend(string header, Client client) { }
    public void ServerEncondeAndSend(string header, IConfig config, Client client) { }
    public void ServerEncondeAndBroadcast(string header, IConfig config) { }
    public void ListenForClients(string header, Action<IReadMessage, Client> callback) { }
#endif

#if SERVER
    public bool IsClient =>false;

    public bool DoesClientHasPermissions(Client client)
      => client.Connection == GameMain.Server.OwnerConnection || client.HasPermission(RequiredPermissions);

    public void ServerSend(string header, Client client)
    {
      GameMain.LuaCs.Networking.Send(
        GameMain.LuaCs.Networking.Start(header),
        client.Connection
      );
    }
    public void ServerEncondeAndSend(string header, IConfig config, Client client)
    {
      IWriteMessage outMsg = GameMain.LuaCs.Networking.Start(header);
      config.NetEncode(outMsg);
      GameMain.LuaCs.Networking.Send(outMsg, client.Connection);
    }
    public void ServerEncondeAndBroadcast(string header, IConfig config)
    {
      IWriteMessage outMsg = GameMain.LuaCs.Networking.Start(header);
      config.NetEncode(outMsg);
      GameMain.LuaCs.Networking.Send(outMsg);
    }

    public void ListenForClients(string header, Action<IReadMessage, Client> callback)
    {
      GameMain.LuaCs.Networking.Receive(header, (object[] args) =>
      {
        callback?.Invoke(args[0] as IReadMessage, args[1] as Client);
      });
    }

    public bool DoIHavePermissions() => true;
    public void ClientSend(string header) { }
    public void ClientEncondeAndSend(string header, IConfig config) { }
    public void ListenForServer(string header, Action<IReadMessage> callback) { }
#endif
  }



}
