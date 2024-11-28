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
    public ModSettings modSettings { get; set; } = new ModSettings();

    public void Apply()
    {
      Vanilla.Apply();
      modSettings.ActualColor = UltimateParser.Parse<Color>(modSettings.AmbienceColor);
    }

    public Settings() { }


  }
}