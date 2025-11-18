#if !CLIENT

using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Runtime.CompilerServices;
using Barotrauma.Networking;

namespace BaroJunk
{
  // BRUH this is massive code duplication, think how to integrate it better
  /// <summary>
  /// This class allows you to request command execution on server via GameMain.LuaCs.Networking
  /// you can also try GameMain.Client.SendConsoleCommand(); 
  /// but then you'll lose information about the Client
  /// and thus won't be able to setup permissions
  /// </summary>
  public partial class CommandRelay
  {
    /// <summary>
    /// So you could customize it, in theory
    /// </summary>
    public Func<Client, bool> DoesClientHavePermissions { get; set; } = (client) => client.Connection == GameMain.Server.OwnerConnection || client.HasPermission(ClientPermissions.ConsoleCommands);

    public string Header { get; set; } = "CommandRelay";


    public IsolatedCommands IsolatedCommands { get; set; } = new IsolatedCommands();

    public Action<Client> OnRequestFailed { get; set; }
    public Action<Client, string> OnRequestSucceed { get; set; }

    public void ListenForCommands()
    {
      GameMain.LuaCs.Networking.Receive(Header, (object[] args) =>
      {
        OnCommandRequested(args[0] as IReadMessage, args[1] as Client);
      });
    }

    public void OnCommandRequested(IReadMessage msg, Client client)
    {
      if (!DoesClientHavePermissions(client))
      {
        OnRequestFailed?.Invoke(client);
        return;
      }

      string command = msg.ReadString();
      if (String.IsNullOrEmpty(command)) return;

      OnRequestSucceed?.Invoke(client, command);
    }

    public void Open() { ListenForCommands(); }

    public CommandRelay()
    {
      Open();
      OnRequestSucceed = (client, command) => IsolatedCommands.Execute(command);
    }
  }
}
#endif