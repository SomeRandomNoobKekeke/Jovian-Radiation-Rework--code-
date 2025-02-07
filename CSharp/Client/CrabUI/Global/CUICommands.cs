using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.IO;

using Barotrauma;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;

namespace CrabUI_JovianRadiationRework
{
  public partial class CUI
  {
    internal static List<DebugConsole.Command> AddedCommands = new List<DebugConsole.Command>();
    internal static void AddCommands()
    {
      AddedCommands.Add(new DebugConsole.Command("cuidebug", "", CUIDebug_Command));
      AddedCommands.Add(new DebugConsole.Command("cuimg", "", CUIMG_Command));
      AddedCommands.Add(new DebugConsole.Command("printsprites", "", PrintSprites_Command));
      AddedCommands.Add(new DebugConsole.Command("printkeys", "", PrintSprites_Command));
      AddedCommands.Add(new DebugConsole.Command("cuipalettedemo", "", PaletteDemo_Command));

      DebugConsole.Commands.InsertRange(0, AddedCommands);
    }

    public static void CUIDebug_Command(string[] args)
    {
      if (CUIDebugWindow.Main == null)
      {
        CUIDebugWindow.Open();
      }
      else
      {
        CUIDebugWindow.Close();
      }
    }

    public static void CUIMG_Command(string[] args) => CUIMagnifyingGlass.ToggleEquip();

    public static void PrintSprites_Command(string[] args)
    {
      foreach (GUIComponentStyle style in GUIStyle.ComponentStyles)
      {
        CUI.Log($"{style.Name} {style.Sprites.Count}");
      }
    }

    public static void PrintKeysCommand(string[] args)
    {
      CUIDebug.PrintKeys = !CUIDebug.PrintKeys;

      if (CUIDebug.PrintKeys)
      {
        var values = typeof(Keys).GetEnumValues();
        foreach (var v in values)
        {
          Log($"{(int)v} {v}");
        }
        Log("---------------------------");
      }
    }

    public static void PaletteDemo_Command(string[] args)
    {
      try { CUIPalette.PaletteDemo(); } catch (Exception e) { CUI.Warning(e); }
    }


    internal static void RemoveCommands()
    {
      AddedCommands.ForEach(c => DebugConsole.Commands.Remove(c));
      AddedCommands.Clear();
    }

    // public static void PermitCommands(Identifier command, ref bool __result)
    // {
    //   if (AddedCommands.Any(c => c.Names.Contains(command.Value))) __result = true;
    // }
  }
}