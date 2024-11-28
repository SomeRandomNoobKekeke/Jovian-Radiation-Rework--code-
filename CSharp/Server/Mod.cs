using System;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public void InitProjSpecific()
    {
      if (IOManager.SettingsExist)
      {
        settingsManager.LoadFromAndUse(IOManager.SettingsFile);
      }
      else
      {
        settingsManager.LoadFromAndUse(IOManager.DefaultPreset);
        settingsManager.SaveTo(IOManager.SettingsFile);
      }

      GameMain.LuaCs.Networking.Receive("jrr_ask", NetManager.Give);
      GameMain.LuaCs.Networking.Receive("jrr_sync", NetManager.Receive);
    }


  }
}