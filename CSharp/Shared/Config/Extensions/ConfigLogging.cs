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

  }
}