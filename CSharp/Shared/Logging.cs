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
    public static void Log(object msg, Color? cl = null)
    {
      cl ??= Color.Cyan;
      LuaCsLogger.LogMessage($"{msg ?? "null"}", cl * 0.8f, cl);
    }

    public static void Info(object msg, [CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    {
      if (instance?.Debug == true)
      {
        var fi = new FileInfo(source);

        Log($"{fi.Directory.Name}/{fi.Name}:{lineNumber}", Color.Cyan * 0.5f);
        Log(msg, Color.Cyan);
      }
    }

    public static void Error(object msg, [CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    {
      if (instance?.Debug == true)
      {
        var fi = new FileInfo(source);

        Log($"{fi.Directory.Name}/{fi.Name}:{lineNumber}", Color.Orange * 0.5f);
        Log(msg, Color.Orange);
      }
    }

    public static void MemoryUsage(object msg)
    {
      if (instance?.Debug == true)
      {
        Log($"Memory Usage ({msg}): {LuaCsPerformanceCounter.MemoryUsage}", Color.Lime);
      }
    }
  }
}
