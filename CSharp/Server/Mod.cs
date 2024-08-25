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
      Settings.save(settings);

      GameMain.LuaCs.Networking.Receive("jrr_init", Settings.net_recieve_init);
      GameMain.LuaCs.Networking.Receive("jrr_sync", Settings.net_recieve_sync);
    }

    public void PatchOnServer()
    {
      harmony.Patch(
        original: typeof(LuaGame).GetMethod("IsCustomCommandPermitted"),
        postfix: new HarmonyMethod(typeof(Mod).GetMethod("permitCommands"))
      );
    }

  }
}