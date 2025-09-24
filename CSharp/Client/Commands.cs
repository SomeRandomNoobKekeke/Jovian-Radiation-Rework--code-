global using BaroJunk;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;


namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public partial void AddCommandsProjSpecific()
    {
      AddedCommands.Add(new DebugConsole.Command("radiation_printmodel", "", (string[] args) =>
      {
        Mod.Logger.Log(CurrentModel);
      }));
    }

  }
}