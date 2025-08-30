using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.IO;

using Barotrauma;
using Microsoft.Xna.Framework;
using HarmonyLib;
using MoonSharp.Interpreter;

namespace JovianRadiationRework
{
  public static class ConfigCommands
  {
    public static DebugConsole.Command Command;

    public static string DefaultCommandName => ConfigManager.CurrentConfig is null ?
      Utils.ModHookId.ToLower() : $"{(ConfigManager.CurrentConfig.GetType().Namespace.ToLower())}";

    public static void Init()
    {
      AdvancedCommand.SetupHooks();

      GameMain.LuaCs.Hook.Add("stop", $"remove {Utils.ModHookId} config command", (object[] args) =>
      {
        if (Command is not null)
        {
          DebugConsole.Commands.Remove(Command);
        }
        return null;
      });

      GameMain.LuaCs.Hook.Patch(Utils.ModHookId + ".PermitConfigCommand", typeof(DebugConsole).GetMethod("IsCommandPermitted", BindingFlags.NonPublic | BindingFlags.Static),
      (object instance, LuaCsHook.ParameterTable ptable) =>
      {
        if (Command is null) return null;

        if (((Identifier)ptable["command"]) == Command.Names[0])
        {
          ptable.ReturnValue = true;
          ptable.PreventExecution = true;
        }

        return null;
      });
    }

    public static void UpdateCommand()
    {
      if (!Utils.AlreadyDone()) Init();

      if (Command is not null)
      {
        DebugConsole.Commands.Remove(Command);
      }

      if (ConfigManager.UseAdvancedCommand)
      {
        Command = new AdvancedCommand(
          ConfigManager.CommandName ?? DefaultCommandName,
          "",
          EditConfig_AdvancedCommand,
          ConfigSerialization.ToAdvancedHints(ConfigManager.CurrentConfig)
        );
      }
      else
      {
        Command = new DebugConsole.Command(
          ConfigManager.CommandName ?? DefaultCommandName,
          $"",
          EditConfig_VanillaCommand,
          ConfigSerialization.ToHints(ConfigManager.CurrentConfig)
        );
      }

      DebugConsole.Commands.Insert(0, Command);
    }

    public static void EditConfig_AdvancedCommand(string[] args)
    {
      if (ConfigManager.CurrentConfig is null)
      {
        Mod.Warning("config is null");
        return;
      }

      if (args.Length == 0)
      {
        Mod.Log(ConfigSerialization.ToText(ConfigManager.CurrentConfig));
        return;
      }


      if (args.Length == 1)
      {
        Mod.Log(ConfigTraverse.Get(ConfigManager.CurrentConfig, args[0]));
        return;
      }

      if (args.Length > 1)
      {
        ConfigEntry entry = ConfigTraverse.Get(ConfigManager.CurrentConfig, string.Join('.', args));
        if (entry.IsValid)
        {
          Mod.Log(entry.Value);
          return;
        }

        entry = ConfigTraverse.Get(ConfigManager.CurrentConfig, string.Join('.', args.SkipLast(1)));
        if (entry.IsValid)
        {
          try
          {
            if (entry.Property.PropertyType.IsAssignableTo(typeof(IConfig)))
            {
              Mod.Warning("That's not a prop");
              return;
            }
            entry.Value = Parser.Parse(args.Last(), entry.Property.PropertyType);
#if CLIENT
            if (GameMain.IsMultiplayer) ConfigNetworking.Sync();
#endif
            Mod.Log($"{string.Join('.', args.SkipLast(1))} = {args.Last()}");
          }
          catch (Exception e)
          {
            Mod.Warning(e.Message);
          }
        }
      }
    }

    public static void EditConfig_VanillaCommand(string[] args)
    {
      if (ConfigManager.CurrentConfig is null)
      {
        Mod.Warning("config is null");
        return;
      }

      if (args.Length == 0)
      {
        Mod.Log(ConfigSerialization.ToText(ConfigManager.CurrentConfig));
        return;
      }

      var flat = ConfigTraverse.GetFlat(ConfigManager.CurrentConfig);

      if (args.Length == 1)
      {
        if (flat.ContainsKey(args[0]))
        {
          Mod.Log(flat[args[0]].Value);
        }
        else
        {
          Mod.Warning("No such prop");
        }
        return;
      }

      if (args.Length > 1)
      {
        if (!flat.ContainsKey(args[0]))
        {
          Mod.Warning("No such prop");
          return;
        }

        ConfigEntry entry = flat[args[0]];
        try
        {
          if (entry.Property.PropertyType.IsAssignableTo(typeof(IConfig)))
          {
            Mod.Warning("That's not a prop");
            return;
          }

          entry.Value = Parser.Parse(args[1], entry.Property.PropertyType);
#if CLIENT
          if (GameMain.IsMultiplayer) ConfigNetworking.Sync();
#endif
        }
        catch (Exception e)
        {
          Mod.Warning(e.Message);
        }
      }
    }

  }
}