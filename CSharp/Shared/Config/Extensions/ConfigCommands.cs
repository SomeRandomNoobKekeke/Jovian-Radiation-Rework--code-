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
    public static object CurrentConfig;
    public static DebugConsole.Command Command;
    public static void Use(object config, string commandName)
    {
#if CLIENT
    UseClient(config, commandName);
#elif SERVER
    UseServer(config, commandName);
#endif
    }

    public static void UseClient(object config, string commandName)
    {
      CurrentConfig = config;
      string id = $"{CurrentConfig.GetType().Namespace}_{CurrentConfig.GetType().Name}";

      if (DebugConsole.Commands.Any(c => c.Names.Contains(commandName)))
      {
        Mod.Warning($"Can't add [{commandName}] command, it already exists");
        return;
      }

      GameMain.LuaCs.Hook.Add("stop", $"remove {id} command", (object[] args) =>
      {
        if (Command == null) return null;
        DebugConsole.Commands.RemoveAll(c => c.Names.Contains(commandName));
        return null;
      });

      Command = new DebugConsole.Command(commandName, $"Access to {id}", EditConfig_Command, EditConfig_Hints);

      DebugConsole.Commands.Insert(0, Command);

      GameMain.LuaCs.Hook.Patch(id, typeof(DebugConsole).GetMethod("IsCommandPermitted", BindingFlags.NonPublic | BindingFlags.Static), (object instance, LuaCsHook.ParameterTable ptable) =>
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

    public static void UseServer(object config, string commandName)
    {

    }

    public static void EditConfig_Command(string[] args)
    {
      if (CurrentConfig is null)
      {
        Mod.Warning("config is null");
        return;
      }

      if (args.Length == 0)
      {
        Mod.Log(ConfigSerialization.ToText(CurrentConfig));
        return;
      }

      var flat = ConfigTraverse.GetFlat(CurrentConfig);

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

    public static string[][] EditConfig_Hints()
    {
      if (CurrentConfig is null) return new string[][] { };
      return new string[][] { ConfigTraverse.GetFlat(CurrentConfig).Keys.ToArray() };
    }
  }
}