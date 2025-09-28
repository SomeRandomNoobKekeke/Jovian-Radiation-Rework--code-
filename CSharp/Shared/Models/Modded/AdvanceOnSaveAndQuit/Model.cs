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
  public partial class AdvanceOnSaveAndQuitModel : RadiationModel
  {
    public class ModelSettings : IConfig
    {
      public bool ProgressOnSaveLoad { get; set; }
    }

    public override ISaveAndQuitHandler SaveAndQuitHandler { get; set; }
  }



}