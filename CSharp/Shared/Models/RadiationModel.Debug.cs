using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using System.Text;
using BaroJunk_Config;


namespace JovianRadiationRework
{

  public partial class RadiationModel
  {
    public bool Debug { get; set; }
    public void DebugLog(object msg)
    {
      if (Debug) Mod.Logger.Log(msg);
    }

  }
}