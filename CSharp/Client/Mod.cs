using System;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Barotrauma.Networking;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static bool HasPermissions => GameMain.IsSingleplayer || GameMain.Client.IsServerOwner || GameMain.Client.HasPermission(ClientPermissions.All);

    public void InitProjSpecific()
    {
      if (GameMain.IsSingleplayer)
      {
        if (IOManager.SettingsExist)
        {
          settingsManager.LoadFrom(IOManager.SettingsFile);
        }
        else
        {
          settingsManager.LoadFrom(IOManager.DefaultPreset);
        }
        settingsManager.SaveTo(IOManager.SettingsFile);
      }

      if (GameMain.IsMultiplayer)
      {
        GameMain.LuaCs.Networking.Receive("jrr_sync", NetManager.Receive);
        NetManager.Ask();
      }
    }

    public void InitializeProjSpecific()
    {

    }

    public void DisposeProjSpecific()
    {
    }

  }


}