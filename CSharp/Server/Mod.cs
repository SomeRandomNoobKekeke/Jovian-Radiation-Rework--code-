using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;


using Barotrauma.Extensions;
using Barotrauma.Networking;
namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public Harmony harmony;

    public void Initialize()
    {
      figureOutModVersionAndDirPath();
      addCommands();

      createFolders();
      settings = Settings.load();
      settings.apply();

      GameMain.LuaCs.Networking.Receive("jrr_init", Settings.net_recieve_init);
      GameMain.LuaCs.Networking.Receive("jrr_sync", Settings.net_recieve_sync);

      info($"{meta.ModName} | {meta.ModVersion} - Compiled");
    }

    public void OnLoadCompleted() { }
    public void PreInitPatching() { }

    public void Dispose()
    {
      serializer = null;
      removeCommands();
    }
  }


}