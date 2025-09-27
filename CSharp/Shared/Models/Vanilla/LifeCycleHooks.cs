using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

using Barotrauma.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Voronoi2;


namespace JovianRadiationRework
{
  public partial class VanillaRadiationModel
  {
    public class VanillaLifeCycleHooks : ILifeCycleHooks
    {
      public void OnLoad()
      {
        Mod.CurrentModel.ElectronicsDamager?.ScanItems();
      }

      public void OnRoundStart()
      {
        Mod.CurrentModel.ElectronicsDamager?.ScanItems();
      }
      public void OnRoundEnd()
      {
        Mod.CurrentModel.ElectronicsDamager?.ForgetItems();
      }
      public void OnSaveAndQuit()
      {

      }
      public void OnQuit()
      {

      }
    }



  }
}