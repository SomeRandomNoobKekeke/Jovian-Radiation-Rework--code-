using System;
using System.Reflection;
using System.Diagnostics;
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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static void log(object msg, Color? cl = null, string line = "")
    {
      if (cl == null) cl = Color.Cyan;

      LuaCsLogger.LogMessage($"{line}{msg ?? "null"}", cl * 0.8f, cl);
    }
    public static void info(object msg, [CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    {
      if (debug)
      {
        var fi = new FileInfo(source);

        log($"{fi.Directory.Name}/{fi.Name}:{lineNumber}", Color.Cyan * 0.5f);
        log(msg, Color.Cyan);
      }
    }
    public static void err(object msg, [CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    {
      if (debug)
      {
        var fi = new FileInfo(source);

        log($"{fi.Directory.Name}/{fi.Name}:{lineNumber}", Color.Orange * 0.5f);
        log(msg, Color.Orange);
      }
    }

    public static string json(Object o, bool indent = false)
    {
      try { return JsonSerializer.Serialize(o, new JsonSerializerOptions { WriteIndented = indent }); }
      catch (Exception e) { err(e); return ""; }
    }

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