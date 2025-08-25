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
  //TODO lol
  public static class Utils
  {
    public static string BarotraumaPath => Path.GetFullPath("./");

    public static ContentPackage ModPackage<PluginType>() where PluginType : IAssemblyPlugin
    {
      GameMain.LuaCs.PluginPackageManager.TryGetPackageForPlugin<PluginType>(out ContentPackage package);
      return package;
    }
    public static string ModDir<PluginType>() where PluginType : IAssemblyPlugin => ModPackage<PluginType>().Dir;
    public static string ModVersion<PluginType>() where PluginType : IAssemblyPlugin => ModPackage<PluginType>().ModVersion;
    public static string ModName<PluginType>() where PluginType : IAssemblyPlugin => ModPackage<PluginType>().Name;
  }
}