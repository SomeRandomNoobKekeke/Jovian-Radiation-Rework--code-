using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace BaroJunk
{
  public static class ExtraParsingMethods
  {
    //FIXME why MethodInfo? why not Func?
    public static Dictionary<Type, MethodInfo> Parse = new()
    {
      [typeof(Vector2)] = typeof(ExtraParsingMethods).GetMethod("ParseVector2"),
      [typeof(Color)] = typeof(ExtraParsingMethods).GetMethod("ParseColor"),
    };
    public static Dictionary<Type, MethodInfo> Serialize = new()
    {
      [typeof(Vector2)] = typeof(ExtraParsingMethods).GetMethod("Vector2ToString"),
      [typeof(Color)] = typeof(ExtraParsingMethods).GetMethod("ColorToString"),
    };

    public static string ColorToString(Color cl) => XMLExtensions.ColorToString(cl);
    public static Color ParseColor(string raw) => XMLExtensions.ParseColor(raw);

    public static string Vector2ToString(Vector2 v) => $"[{v.X},{v.Y}]";
    public static Vector2 ParseVector2(string raw)
    {
      if (raw == null || raw == "") return new Vector2(0, 0);

      string content = raw.Split('[', ']')[1];

      List<string> coords = content.Split(',').Select(s => s.Trim()).ToList();

      float x = 0;
      float y = 0;

      float.TryParse(coords.ElementAtOrDefault(0), out x);
      float.TryParse(coords.ElementAtOrDefault(1), out y);

      return new Vector2(x, y);
    }
  }
}