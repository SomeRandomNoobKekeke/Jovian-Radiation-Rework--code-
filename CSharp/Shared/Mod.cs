using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

using Barotrauma.Extensions;
using System.IO;
using Barotrauma.Networking;
namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static ModMetadata meta = new ModMetadata();
    public static Settings settings = new Settings();
    public static bool debug = true;

    public static void log(object msg, Color? cl = null, string line = "")
    {
      if (cl == null) cl = Color.Cyan;
#if SERVER
      cl *= 0.8f;
#endif
      LuaCsLogger.LogMessage($"{line}{msg ?? "null"}", cl, cl);
    }
    public static void info(object msg, [CallerLineNumber] int lineNumber = 0) { if (debug) log(msg, Color.Cyan, $"{lineNumber}| "); }
    public static void err(object msg, [CallerLineNumber] int lineNumber = 0) { if (debug) log(msg, Color.Orange, $"{lineNumber}| "); }

    public void figureOutModVersionAndDirPath()
    {
      bool found = false;

      foreach (ContentPackage p in ContentPackageManager.EnabledPackages.All)
      {
        if (p.Name.Contains(meta.ModName))
        {
          found = true;
          meta.ModVersion = p.ModVersion;
          meta.ModDir = Path.GetFullPath(p.Dir);
          break;
        }
      }

      if (!found) err($"Couldn't figure out {meta.ModName} mod folder");
    }
  }


}