#define CLIENT

using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;
using Barotrauma.Networking;

namespace BaroJunk
{

  public partial interface IConfig
  {
    public string NetHeader => ID;
    public void NetEncode(IWriteMessage msg)
    {
      foreach (ConfigEntry entry in GetEntriesRec())
      {
        NetParser.Encode(msg, entry);
      }
    }

    public void NetDecode(IReadMessage msg)
    {
      foreach (ConfigEntry entry in GetEntriesRec())
      {
        entry.Value = NetParser.Decode(msg, entry.Type).Result;
      }
    }

    //TODO uncringe
    #region Cringe

#if CLIENT
    public SimpleResult Ask()
    {
      if (!Facades.NetFacade.IsMultiplayer) return SimpleResult.Failure("It's not multiplayer");
      Facades.NetFacade.ClientSend(NetHeader + "_ask");
      return SimpleResult.Success();
    }

    public SimpleResult Sync()
    {
      if (!Facades.NetFacade.IsMultiplayer) return SimpleResult.Failure("It's not multiplayer");

      //TODO why ConsoleCommands permission is hardcoded here?
      if (!Facades.NetFacade.DoIHavePermissions()) return SimpleResult.Failure(
        "You need to be the host or have ConsoleCommands permission to use it"
      );

      Facades.NetFacade.ClientEncondeAndSend(NetHeader + "_sync", this);
      return SimpleResult.Success();
    }

#elif SERVER

    public SimpleResult Sync()
    {
      if (!Facades.NetFacade.IsMultiplayer) return SimpleResult.Failure("It's not multiplayer");
      Facades.NetFacade.ServerEncondeAndBroadcast(NetHeader + "_sync", this);
      return SimpleResult.Success();
    }
#endif


    #endregion


  }

}