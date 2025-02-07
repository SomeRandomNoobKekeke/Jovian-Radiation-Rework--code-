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
using CrabUI_JovianRadiationRework;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static bool HasPermissions => GameMain.Client.IsServerOwner || GameMain.Client.HasPermission(ClientPermissions.All);

    public SettingsUI UI;

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
      CUI.PGNAssets = Path.Combine(ModDir, "Assets");
      CUI.Initialize();
      CUIPalette.Current = CUIPalette.Radiation;

      UI = new SettingsUI();
    }

    public void DisposeProjSpecific()
    {
      CUI.Dispose();
    }

  }


}