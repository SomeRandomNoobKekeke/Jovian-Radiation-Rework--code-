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
    public event Action<Settings> OnSettinsChangedFromConsole;

    public void AddProjSpecificCommands()
    {
      AddedCommands.Add(new DebugConsole.Command("rad_info", "some info", Rad_Info_Command));
      AddedCommands.Add(new DebugConsole.Command("rad_metadata", "", Rad_Metadata_Command));
      AddedCommands.Add(new DebugConsole.Command("rad_debug", "", Rad_Debug_Command));
      AddedCommands.Add(new DebugConsole.Command("rad", "rad variable [value]", (string[] args) =>
      {
        Rad_Command(args);
        OnSettinsChangedFromConsole?.Invoke(settingsManager.Current);
      }, () => new string[][] { Settings.flatView.Props.Keys.ToArray() }));
      AddedCommands.Add(new DebugConsole.Command("rad_load", "rad_load [name]", Rad_Load_Command, () => new string[][] { IOManager.AllPresets().Keys.ToArray() }));
      AddedCommands.Add(new DebugConsole.Command("rad_delete", "rad_delete [name]", Rad_Delete_Command, () => new string[][] { IOManager.AllPresets().Keys.ToArray() }));

      AddedCommands.Add(new DebugConsole.Command("rad_save", "rad_save [name]", Rad_Save_Command));
      AddedCommands.Add(new DebugConsole.Command("rad_amount", "rad_amount [value]", Rad_Amount_Command));
      AddedCommands.Add(new DebugConsole.Command("rad_gui", "opens the gui", Rad_GUI_Command));

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


    public void Rad_Info_Command(string[] args)
    {
      try
      {
        if (Screen.Selected.Cam != null)
        {
          Mod.Log($"Camera Irradiation: {CameraIrradiation(Screen.Selected.Cam)}");
        }

        if (Screen.Selected.Cam != null)
        {
          Mod.Log($"Main Sub Irradiation: {EntityRadiationAmount(Submarine.MainSub)}");
        }

        if (GameMain.GameSession?.Map?.CurrentLocation != null)
        {
          Mod.Log($"Current Location Irradiation: {CurrentLocationRadiationAmount()}");
        }

        // if (GameMain.GameSession?.Map?.CurrentLocation != null)
        // {
        //   Mod.Log($"Start Location Irradiation: {LocationIrradiation(Level.Loaded?.StartLocation)}");
        // }

        // if (GameMain.GameSession?.Map?.CurrentLocation != null)
        // {
        //   Mod.Log($"End Location Irradiation: {LocationIrradiation(Level.Loaded?.EndLocation)}");
        // }
      }
      catch (Exception e)
      {
        Mod.Warning(e);
      }
    }

    public void Rad_Metadata_Command(string[] args)
    {
      try
      {
        object o = GetMetadata("CurrentLocationIrradiation") ?? "null";
        Mod.Log($"CurrentLocationIrradiation: {o}");

        // o = GetMetadata("MainSubIrradiation") ?? "null";
        // Mod.Log($"MainSubIrradiation: {o}");
      }
      catch (Exception e)
      {
        Mod.Warning(e);
      }
    }


    public void Rad_Debug_Command(string[] args)
    {
      Mod.Instance.Debug = !Mod.Instance.Debug;
      Mod.Log($"Radiation Debug: {Mod.Instance.Debug}");
    }

    public void Rad_Command(string[] args)
    {
      try
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
      }
      catch (Exception e)
      {
        Mod.Warning(e);
      }
    }

    public void Rad_Load_Command(string[] args)
    {
      try
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
          Mod.Instance.settingsManager.LoadFrom(targetPath);
          Mod.Instance.settingsManager.SaveTo(IOManager.SettingsFile);
          Mod.Log($"Loaded {Mod.settings.Name}");
          OnSettinsChangedFromConsole?.Invoke(settingsManager.Current);
        }

        if (GameMain.IsMultiplayer)
        {
          if (HasPermissions)
          {
            Mod.Instance.settingsManager.JustLoad(targetPath);
            NetManager.Sync(Mod.settings);
            Mod.Log($"Loaded {Mod.settings.Name}");
            OnSettinsChangedFromConsole?.Invoke(settingsManager.Current);
          }
          else
          {
            Mod.Log($"Only Host or players with all permissions can use this");
          }
        }
      }
      catch (Exception e)
      {
        Mod.Warning(e);
      }
    }

    public void Rad_Save_Command(string[] args)
    {
      try
      {
        string targetPath = IOManager.SettingsFile;

        if (args.Length != 0)
        {
          targetPath = Path.Combine(IOManager.SavedPresets, args[0] + ".xml");
        }

        Mod.Instance.settingsManager.SaveTo(targetPath);

        Mod.Log($"Saved to {targetPath}");
      }
      catch (Exception e)
      {
        Mod.Warning(e);
      }
    }


    public void Rad_Delete_Command(string[] args)
    {
      try
      {
        string targetPath = null;

        if (args.Length == 0)
        {
          Mod.Log($"delete what?");
          return;
        }

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

        if (targetPath == null)
        {
          Mod.Log("Not found");
          return;
        }

        if (File.Exists(targetPath))
        {
          File.Delete(targetPath);
          Mod.Log($"{targetPath} deleted");
        }

      }
      catch (Exception e)
      {
        Mod.Warning(e);
      }
    }

    public void Rad_Amount_Command(string[] args)
    {
      try
      {
        if (args.Length != 0 && GameMain.GameSession?.Map?.Radiation != null)
        {
          if (float.TryParse(args[0], out float amount))
          {
            GameMain.GameSession.Map.Radiation.Amount = amount;
            SetMetadata("CurrentLocationIrradiation", CurrentLocationRadiationAmount());

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
      }
      catch (Exception e)
      {
        Mod.Warning(e);
      }
    }

    public void Rad_GUI_Command(string[] args)
    {
      try
      {
        if (Mod.Instance == null) return;
        Mod.Instance.UI.OpenSettingsFrame();
      }
      catch (Exception e)
      {
        Mod.Error(e);
      }
    }
  }
}