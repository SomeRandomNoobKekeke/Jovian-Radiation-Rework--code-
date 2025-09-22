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
  //TODO idk, seems redundant
  public static class ExtraXMLParsingMethods
  {
    public static Dictionary<Type, MethodInfo> Parse = new()
    {
      // [typeof(Vector2)] = typeof(ExtraParsingMethods).GetMethod("ParseVector2"),
    };
    public static Dictionary<Type, MethodInfo> Serialize = new()
    {
      // [typeof(Vector2)] = typeof(ExtraParsingMethods).GetMethod("Vector2ToString"),
    };


  }
}