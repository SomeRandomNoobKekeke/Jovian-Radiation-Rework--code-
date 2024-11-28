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

    public static object Parse(Type targetType, string value)
    {
      if (value.GetType() != typeof(string))
      {
        Mod.Info($"Can't parse {value} into {targetType}");
        return GetDefault(targetType);
      }

      if (targetType == typeof(string))
      {
        return value;
      }

      object result;

      MethodInfo parse = targetType.GetMethod("Parse", AccessTools.all, new Type[]{
        typeof(string)
      });

      if (parse != null)
      {
        try
        {
          result = parse.Invoke(null, new object[] { value });
        }
        catch (Exception e)
        {
          Mod.Info($"Can't parse {value} into {targetType}");
          result = GetDefault(targetType);
        }
      }
      else if (AditionalParseMethods.ContainsKey(targetType))
      {
        try
        {
          result = AditionalParseMethods[targetType](value);
        }
        catch (Exception e)
        {
          Mod.Info($"AditionalParseMethods failed while trying to parse {value} into {targetType}");
          result = GetDefault(targetType);
        }
      }
      else
      {
        result = GetDefault(targetType);
      }

      return result;
    }

  }
}