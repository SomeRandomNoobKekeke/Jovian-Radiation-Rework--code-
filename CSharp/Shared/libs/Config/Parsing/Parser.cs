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

  /// <summary>
  /// Parse primitive types, can be extended with ExtraParsingMethods
  /// </summary>
  public static class Parser
  {
    /// <summary>
    /// Null is serialized into this, so you could distinguish null and empty string
    /// </summary>
    public static string NullTerm = "{{null}}";
    public static bool Verbose = true;
    public static object DefaultFor(Type T)
    {
      try
      {
        if (T == typeof(string)) return null;//HACK
        return Activator.CreateInstance(T);
      }
      catch (Exception e)
      {
        if (Verbose)
        {
          Mod.Warning($"-- Failed to CreateInstance of type [{T}] because: [{e.Message}], setting it to null");
        }
        return null;
      }
    }


    public static T Parse<T>(string raw) => (T)Parse(raw, typeof(T));
    public static object Parse(string raw, Type T)
    {
      if (raw == null) return null;
      if (raw == NullTerm) return null;
      if (T == typeof(string)) return raw;

      if (T.IsPrimitive)
      {
        MethodInfo parse = T.GetMethod(
          "Parse",
          BindingFlags.Public | BindingFlags.Static,
          new Type[] { typeof(string) }
        );

        try
        {
          return parse.Invoke(null, new object[] { raw });
        }
        catch (Exception e)
        {
          if (Verbose)
          {
            Mod.Warning($"-- Parser couldn't parse [{raw}] into primitive type [{T}] because [{e.Message}]");
          }
          return DefaultFor(T);
        }
      }

      if (T.IsEnum)
      {
        try
        {
          return Enum.Parse(T, raw);
        }
        catch (Exception e)
        {
          if (Verbose)
          {
            Mod.Warning($"-- Parser couldn't parse [{raw}] into Enum [{T}] because [{e.Message}]");
          }
          return DefaultFor(T);
        }
      }

      if (!T.IsPrimitive)
      {
        MethodInfo parse = null;
        if (ExtraParsingMethods.Parse.ContainsKey(T))
        {
          parse = ExtraParsingMethods.Parse[T];
        }
        else
        {
          parse = T.GetMethod(
            "Parse",
            BindingFlags.Public | BindingFlags.Static,
            new Type[] { typeof(string) }
          );
        }

        if (parse == null)
        {
          if (Verbose)
          {
            Mod.Warning($"-- Parser couldn't parse [{raw}] into [{T}] because it doesn't have the Parse method");
          }
          return DefaultFor(T);
        }

        try
        {
          return parse.Invoke(null, new object[] { raw });
        }
        catch (Exception e)
        {
          if (Verbose)
          {
            Mod.Warning($"-- Parser couldn't parse [{raw}] into [{T}] because [{e.Message}]");
          }
          return DefaultFor(T);
        }
      }

      return DefaultFor(T);
    }

    public static string Serialize(object o)
    {
      if (o is null) return NullTerm;
      if (o.GetType() == typeof(string)) return (string)o;


      MethodInfo serialize = ExtraParsingMethods.Serialize.GetValueOrDefault(o.GetType());
      string result = null;

      try
      {
        result = serialize == null ? o.ToString() : (string)serialize.Invoke(null, new object[] { o });
      }
      catch (Exception e)
      {
        if (Verbose) Mod.Warning($"-- Parser couldn't serialize object of [{o.GetType()}] type because [{e.Message}]");
      }

      return result;
    }
  }
}
