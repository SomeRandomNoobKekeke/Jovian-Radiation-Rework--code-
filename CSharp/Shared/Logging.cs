using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using System.IO;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public static string WrapInColor(object o, string cl)
    {
      return $"‖color:{cl}‖{o}‖end‖";
    }

    public static void Log(object msg, Color? cl = null)
    {
      cl ??= Color.Cyan;
      LuaCsLogger.LogMessage($"{msg ?? "null"}", cl * 0.8f, cl);
    }

    public static void Warning(object msg, Color? cl = null)
    {
      cl ??= Color.Yellow;
      LuaCsLogger.LogMessage($"{msg ?? "null"}", cl * 0.8f, cl);
    }

    public static void Info(object msg, [CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    {
      if (Instance?.Debug == true)
      {
        var fi = new FileInfo(source);

        Log($"{fi.Directory.Name}/{fi.Name}:{lineNumber}", Color.Cyan * 0.5f);
        Log(msg, Color.Cyan);
      }
    }

    public static void Error(object msg, [CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    {
      if (Instance?.Debug == true)
      {
        var fi = new FileInfo(source);

        Log($"{fi.Directory.Name}/{fi.Name}:{lineNumber}", Color.Orange * 0.5f);
        Log(msg, Color.Orange);
      }
    }

    public static bool Assert(bool ok, string msg)
    {
      if (!ok) Error(msg);
      return ok;
    }

    public static void MemoryUsage(object msg)
    {
      if (Instance?.Debug == true)
      {
        Log($"Memory Usage ({msg}): {LuaCsPerformanceCounter.MemoryUsage}", Color.Lime);
      }
    }
  }
}
