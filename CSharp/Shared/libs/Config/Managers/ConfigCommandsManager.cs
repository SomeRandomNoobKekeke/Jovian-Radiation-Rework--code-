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

namespace BaroJunk
{
  public class ConfigCommandsManager
  {
    public IConfig Config;
    public ConfigCommandsManager(IConfig config) => Config = config;

    private string commandName; public string CommandName
    {
      get => commandName;
      set
      {
        commandName = value;
        UpdateCommand(commandName);
      }
    }

    public DebugConsole.Command Command;

    private void UpdateCommand(string name)
    {
      if (Command is not null) Config.Facades.ConsoleFacade.Remove(Command);
      if (name is null) return;

      Command = new DebugConsole.Command(CommandName, "", EditConfig_VanillaCommand, Config.ToHints());
      Config.Facades.ConsoleFacade.Insert(Command);
    }

    private void AddHooks()
    {
      Config.Facades.HooksFacade.AddHook("stop", $"remove {Config.ID} config command", (object[] args) =>
      {
        if (Command is not null)
        {
          DebugConsole.Commands.Remove(Command);
        }
        return null;
      });


      Config.Facades.HooksFacade.Patch(
        Config.ID + ".PermitConfigCommand",
        typeof(DebugConsole).GetMethod("IsCommandPermitted", BindingFlags.NonPublic | BindingFlags.Static),
        (object instance, LuaCsHook.ParameterTable ptable) =>
        {
          if (Command is null) return null;

          if (((Identifier)ptable["command"]) == Command.Names[0])
          {
            ptable.ReturnValue = true;
            ptable.PreventExecution = true;
          }

          return null;
        }
      );
    }

    public void EditConfig_VanillaCommand(string[] args)
    {
      if (args.Length == 0)
      {
        Config.Logger.Log(Config.ToText());
        return;
      }

      var flat = Config.GetFlat();

      if (args.Length == 1)
      {
        if (flat.ContainsKey(args[0]))
        {
          Config.Logger.Log(flat[args[0]].Value);
        }
        else
        {
          Config.Logger.Warning("No such prop");
        }
        return;
      }

      if (args.Length > 1)
      {
        if (!flat.ContainsKey(args[0]))
        {
          Config.Logger.Warning("No such prop");
          return;
        }

        try
        {
          SimpleResult result = Parser.Parse(args[1], flat[args[0]].Type);
          if (result.Ok)
          {
            flat[args[0]].Value = result.Result;
            if (GameMain.IsMultiplayer) Config.Sync();
          }
          else
          {
            Config.Logger.Warning(result.Details);
          }
        }
        catch (Exception e)
        {
          Config.Logger.Warning(e.Message);
        }
      }
    }









  }
}