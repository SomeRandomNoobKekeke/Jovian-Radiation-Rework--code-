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


namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public void AddProjSpecificCommands()
    {
      AddedCommands.Add(new DebugConsole.Command("rad_serv_amount", "", (string[] args) =>
      {
        if (args.Length == 0 || !float.TryParse(args[0], out float amount)) return;

        GameMain.GameSession.Map.Radiation.Amount = amount;
      }));

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
          settingsManager.SetProp(name, value);
          NetManager.Broadcast(Mod.settings);
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

        Mod.Instance.settingsManager.LoadFrom(targetPath);
        Mod.Instance.settingsManager.SaveTo(IOManager.SettingsFile);
        NetManager.Broadcast(Mod.settings);
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

      //TODO i need more sync net events for this
      // AddedCommands.Add(new DebugConsole.Command("rad_amount", "rad_amount [value]", (string[] args) =>
      // {
      //   if (args.Length != 0 && GameMain.GameSession?.Map?.Radiation != null)
      //   {
      //     if (float.TryParse(args[0], out float amount))
      //     {
      //       GameMain.GameSession.Map.Radiation.Amount = amount;
      //     }
      //   }

      //   Mod.Log($"Radiation.Amount = {GameMain.GameSession?.Map?.Radiation.Amount}");
      // }));
    }
  }
}