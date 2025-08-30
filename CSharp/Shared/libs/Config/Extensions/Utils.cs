using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Runtime.CompilerServices;

namespace JovianRadiationRework
{
  //TODO put it somewhere sensible
  public static class Utils
  {
    /// <summary>
    /// Hash set to track stuff that should be done once, without creating 1000 bool flags
    /// mb should be in its own class
    /// </summary>
    public static HashSet<string> Once = new();
    public static bool AlreadyDone([CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    {
      if (Once.Contains($"{source}:{lineNumber}")) return true;
      Once.Add($"{source}:{lineNumber}");
      return false;
    }

    public static string ModHookId => Assembly.GetExecutingAssembly().GetName().Name;
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