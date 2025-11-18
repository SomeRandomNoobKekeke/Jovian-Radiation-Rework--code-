#if !SERVER
using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using Barotrauma.Networking;
using System.Runtime.CompilerServices;

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
    public Func<bool> GetIsPermitted { get; set; } = () => GameMain.IsSingleplayer || GameMain.Client?.IsServerOwner == true || GameMain.Client?.HasPermission(ClientPermissions.ConsoleCommands) == true;

    public string Header { get; set; } = "CommandRelay";

    public SimpleResult SendCommand(string commandName, string[] args)
      => SendCommand($"{commandName} {String.Join(' ', args)}");

    public SimpleResult SendCommand(string command)
    {
      if (String.IsNullOrEmpty(command)) return SimpleResult.Failure("empty command");
      if (!GetIsPermitted()) return SimpleResult.Failure("you don't have permissions");

      IWriteMessage outMsg = GameMain.LuaCs.Networking.Start(Header);
      outMsg.WriteString(command);
      GameMain.LuaCs.Networking.Send(outMsg);

      return SimpleResult.Success();
    }

    public void Open() { }

    public CommandRelay()
    {
      Open();
    }
  }
}

#endif