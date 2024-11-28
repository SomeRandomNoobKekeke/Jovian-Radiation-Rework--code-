using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;


using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace JovianRadiationRework
{
  public static class UltimateParser
  {
    public static Dictionary<Type, Func<string, object>> AditionalParseMethods = new Dictionary<Type, Func<string, object>>{
      {typeof(Color), s => XMLExtensions.ParseColor(s)},
    };

    public static object GetDefault(Type type)
    {
      if (type.IsValueType)
      {
        return Activator.CreateInstance(type);
      }
      return null;
    }

    // Note: this is intentionally unsafe
    // because setting value to default when you can't parse it is very sneaky
    // this should be handled upstream
    public static object Parse(Type targetType, string value)
    {
      if (targetType == typeof(string)) return value;

      object result = value;

      MethodInfo parse = targetType.GetMethod("Parse", AccessTools.all, new Type[]{
        typeof(string)
      });

      try
      {
        if (parse != null)
        {
          result = parse.Invoke(null, new object[] { value });
        }
        else if (AditionalParseMethods.ContainsKey(targetType))
        {
          result = AditionalParseMethods[targetType](value);
        }
      }
      catch (Exception e)
      {
        result = value;
      }

      return result;
    }

  }
}