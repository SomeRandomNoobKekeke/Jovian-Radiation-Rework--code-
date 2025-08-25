using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;

using Barotrauma;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public static class ConfigLogging
  {
    public static string ToText(this Dictionary<string, object> dict)
    {
      StringBuilder sb = new StringBuilder();

      sb.Append("{\n");
      foreach (string key in dict.Keys)
      {
        sb.Append($"    {key}: [{Mod.WrapInColor(dict[key], "white")}],\n");
      }
      sb.Append("}");

      return sb.ToString();
    }

    public static string ToText(this IEnumerable<object> arr)
      => $"[\n{String.Join(",\n", arr.Select(ce => ce.ToString()))}\n]";

    public static void Log(params object[] args)
    {
      if (args.Length == 1)
      {
        LuaCsLogger.LogMessage($"{args[0] ?? "null"}", Color.Cyan * 0.8f, Color.Cyan);
      }
      if (args.Length > 1)
      {
        LuaCsLogger.LogMessage($"[{String.Join(", ", args.Select(o => o?.ToString()))}]", Color.Cyan * 0.8f, Color.Cyan);
      }
    }

    public static void DebugLog(params object[] args)
    {
      if (!ConfigManager.Debug) return;
      Log(args);
    }

  }
}