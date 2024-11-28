using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace JovianRadiationRework
{
  public partial class Settings
  {
    public static FlatView flatView = new FlatView(typeof(Settings));
    public VanillaSettings Vanilla { get; set; } = new VanillaSettings();
    public string bebe { get; set; } = "123";
    public int huhu { get; set; } = 123;

    public void Apply() => Vanilla.Apply();

    public Settings() { }


  }
}