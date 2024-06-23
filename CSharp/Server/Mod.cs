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
using System.Text.Json;
using System.Text.Json.Serialization;
namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {

    public void InitializeServer()
    {
      addCommands();

      settings = Settings.load();
      settings.apply();

      GameMain.LuaCs.Networking.Receive("jrr_init", Settings.net_recieve_init);
      GameMain.LuaCs.Networking.Receive("jrr_sync", Settings.net_recieve_sync);
    }

    public void PatchOnServer()
    {

    }

    public void OnLoadCompleted() { }
    public void PreInitPatching() { }

    public void Dispose()
    {
      harmony.UnpatchAll(harmony.Id);
      harmony = null;
      removeCommands();
    }
  }


}