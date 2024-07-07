using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Barotrauma.Networking;
using Barotrauma.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static bool HasPermissions => GameMain.Client.IsServerOwner || GameMain.Client.HasPermission(ClientPermissions.All);

    public void InitializeClient()
    {
      addCommands();

      if (GameMain.IsSingleplayer)
      {
        settings = Settings.load();
        settings.apply();
        Settings.save(settings);
      }

      if (GameMain.IsMultiplayer)
      {
        GameMain.LuaCs.Networking.Receive("jrr_init", Settings.net_recieve_init);
        GameMain.LuaCs.Networking.Receive("jrr_sync", Settings.net_recieve_sync);

        Settings.askServerForSettings();
      }
    }

    public void PatchOnClient()
    {
      harmony.Patch(
        original: typeof(Level).GetMethod("DrawBack", AccessTools.all),
        postfix: new HarmonyMethod(typeof(Mod).GetMethod("Level_DrawBack_Postfix"))
      );

      harmony.Patch(
        original: typeof(LuaGame).GetMethod("IsCustomCommandPermitted"),
        postfix: new HarmonyMethod(typeof(Mod).GetMethod("permitCommands"))
      );
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