using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace JovianRadiationRework
{
  public static class ConfigManager
  {
    private static object currentConfig; public static object CurrentConfig
    {
      get => currentConfig;
      set { currentConfig = value; Use(value); }
    }

    private static void Use(object config)
    {

    }
  }
}