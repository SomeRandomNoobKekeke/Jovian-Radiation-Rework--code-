using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using System.Text;



namespace JovianRadiationRework
{

  public partial class RadiationModel
  {
    public virtual bool Debug { get; set; } = false;
    public void DebugLog(object msg)
    {
      if (Debug) Mod.Logger.Log($"{Logger.WrapInColor(Name, "white")}| {msg}");
    }

  }
}