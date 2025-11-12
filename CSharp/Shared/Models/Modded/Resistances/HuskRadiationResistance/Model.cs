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
  public partial class HuskRadiationResistanceModel : RadiationModel
  {
    public class ModelSettings : IConfig
    {

    }
    public override IHuskResistanceCalculator HuskResistanceCalculator { get; set; }
  }



}