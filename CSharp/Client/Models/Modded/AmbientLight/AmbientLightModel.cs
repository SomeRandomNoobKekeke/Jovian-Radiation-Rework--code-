using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace JovianRadiationRework
{
  public partial class AmbientLightModel
  {
    public partial class ModelSettings : IConfig
    {

    }


    public override ILevelPostDrawer LevelPostDrawer { get; set; }
  }



}