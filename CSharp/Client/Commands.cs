using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.IO;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public void AddProjSpecificCommands()
    {
      AddedCommands.Add(new DebugConsole.Command("rad", "rad variable [value]", (string[] args) =>
      {
        if (args.Length == 0)
        {
          Log("rad variable [value]");
          return;
        }

        string name = args.ElementAtOrDefault(0);
        string value = args.ElementAtOrDefault(1);

        // TODO what if prop doesn't exist?
        if (value != null)
        {
          if (GameMain.IsSingleplayer)
          {
            settingsManager.SetProp(name, value);
            Mod.Instance.settingsManager.SaveTo(IOManager.SettingsFile);
          }

          if (GameMain.IsMultiplayer)
          {
            if (HasPermissions)
            {
              settingsManager.SetProp(name, value);
              NetManager.Sync(Mod.settings);
            }
            else
            {
              Mod.Log($"Only Host or players with all permissions can use this");
            }
          }
        }

        Log($"{Settings.flatView.Props.GetValueOrDefault(name)?.Name} = {settingsManager.GetProp(name)}");
      }, () => new string[][] { Settings.flatView.Props.Keys.ToArray() }));





      AddedCommands.Add(new DebugConsole.Command("rad_load", "rad_load [name]", (string[] args) =>
      {
        string targetPath = null;

        if (args.Length == 0)
        {
          targetPath = IOManager.SettingsFile;
        }
        else
        {
          var presets = IOManager.AllPresets();

          if (presets.ContainsKey(args[0]))
          {
            targetPath = presets[args[0]];
          }
          else
          {
            foreach (string key in presets.Keys)
            {
              if (String.Equals(key, args[0], StringComparison.OrdinalIgnoreCase))
              {
                targetPath = presets[key];
              }
            }
          }
        }

        if (targetPath == null)
        {
          Mod.Log("Not found");
          return;
        }


        if (GameMain.IsSingleplayer)
        {
          Mod.Instance.settingsManager.LoadFromAndUse(targetPath);
          Mod.Log($"Loaded {Mod.settings.Name}");
        }

        if (GameMain.IsMultiplayer)
        {
          if (HasPermissions)
          {
            Mod.Instance.settingsManager.LoadFromAndUse(targetPath);
            NetManager.Sync(Mod.settings);
            Mod.Log($"Loaded {Mod.settings.Name}");
          }
          else
          {
            Mod.Log($"Only Host or players with all permissions can use this");
          }
        }
      }, () => new string[][] { IOManager.AllPresets().Keys.ToArray() }));


      AddedCommands.Add(new DebugConsole.Command("rad_save", "rad_save [name]", (string[] args) =>
      {
        string targetPath = IOManager.SettingsFile;

        if (args.Length != 0)
        {
          targetPath = Path.Combine(IOManager.SavedPresets, args[0] + ".xml");
        }

        Mod.Instance.settingsManager.SaveTo(targetPath);

        Mod.Log($"Saved to {targetPath}");
      }));

      AddedCommands.Add(new DebugConsole.Command("rad_amount", "rad_amount [value]", (string[] args) =>
      {
        if (args.Length != 0 && GameMain.GameSession?.Map?.Radiation != null)
        {
          if (float.TryParse(args[0], out float amount))
          {
            GameMain.GameSession.Map.Radiation.Amount = amount;

            if (GameMain.IsMultiplayer)
            {
              if (HasPermissions)
              {
                DebugConsole.ExecuteCommand($"rad_serv_amount {amount}");
              }
              else
              {
                Mod.Log($"Only Host or players with all permissions can use this");
              }
            }
          }
        }

        Mod.Log($"Radiation.Amount = {GameMain.GameSession?.Map?.Radiation.Amount}");
      }));


      if (Debug)
      {
        AddedCommands.Add(new DebugConsole.Command("rad_runtest", "rad test", (string[] args) =>
        {
          if (args.Length > 0)
          {
            Test.Run(args[0]);
          }
        }, () =>
        {
          Assembly CallingAssembly = Assembly.GetAssembly(typeof(Test));

          string[] allTest = CallingAssembly.GetTypes()
            .Where(T => T.IsSubclassOf(typeof(Test)))
            .Select(T => T.Name).Append("all").ToArray();

          return new string[][] { allTest };
        }));
      }
    }
  }
}