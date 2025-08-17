using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using Barotrauma;
using Microsoft.Xna.Framework;
using System.IO;

namespace JovianRadiationRework
{
  public partial class Mod
  {
    /// <summary>
    /// $"‖color:{color}‖{msg}‖end‖"
    /// </summary>
    public static string WrapInColor(object msg, string color)
      => $"‖color:{color}‖{msg}‖end‖";

    /// <summary>
    /// Serializes the array.  
    /// Obsolete. Use IEnumerable.Print instead
    /// </summary>
    public static string ArrayToString(IEnumerable<object> array)
      => $"[{String.Join(", ", array.Select(o => o.ToString()))}]";

    public static void LogArray(IEnumerable<object> array, Color? color = null)
      => Log(ArrayToString(array), color);

    /// <summary>
    /// Prints a message to console
    /// </summary>
    public static void Log(params object[] args)
    {
      if (args.Length == 1)
      {
        LuaCsLogger.LogMessage($"{args[0] ?? "null"}", Color.Cyan * 0.8f, Color.Cyan);
      }
      if (args.Length > 1)
      {
        LuaCsLogger.LogMessage(ArrayToString(args), Color.Cyan * 0.8f, Color.Cyan);
      }
    }
    public static void Print(object msg, Color? color = null)
    {
      color ??= Color.Cyan;
      LuaCsLogger.LogMessage($"{msg ?? "null"}", color * 0.8f, color);
    }
    public static void Warning(object msg) => Log(msg, Color.Yellow);
    public static void Error(object msg) => Log(msg, Color.Red);

    private static Dictionary<string, int> Traced = new();
    public static void AddTracer(string key, int i = 1) => Traced[key] = i;
    public static bool Trace(string key)
    {
      if (Traced.ContainsKey(key))
      {
        Traced[key]--;
        if (Traced[key] <= 0) Traced.Remove(key);
        return true;
      }
      return false;
    }

    public static void Point([CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    {
      var fi = new FileInfo(source);
      Log($"{fi.Directory.Name}/{fi.Name}:{lineNumber}", Color.Magenta);
    }

    public static void PrintStackTrace()
    {
      StackTrace st = new StackTrace(true);
      for (int i = 0; i < st.FrameCount; i++)
      {
        StackFrame sf = st.GetFrame(i);
        if (sf.GetMethod().DeclaringType is null)
        {
          Log($"-> {sf.GetMethod().DeclaringType?.Name}.{sf.GetMethod()}");
          break;
        }
        Log($"-> {sf.GetMethod().DeclaringType?.Name}.{sf.GetMethod()}");
      }
    }


    /// <summary>
    /// xd
    /// </summary>
    /// <param name="source"> This should be injected by compiler, don't set </param>
    public static string GetCallerFolderPath([CallerFilePath] string source = "") => Path.GetDirectoryName(source);

    /// <summary>
    /// Prints debug message with source path
    /// </summary>
    public static void Info(object msg, [CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    {
      var fi = new FileInfo(source);

      Log($"{fi.Directory.Name}/{fi.Name}:{lineNumber}", Color.Yellow * 0.5f);
      Log(msg, Color.Yellow);
    }
  }
}
