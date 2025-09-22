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

namespace BaroJunk
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
    public static object DefaultFor(Type T)
    {
      if (T == typeof(string)) return null;//HACK
      return Activator.CreateInstance(T);
    }


    public static SimpleResult Parse<T>(string raw) => Parse(raw, typeof(T));
    public static SimpleResult Parse(string raw, Type T)
    {
      if (raw == null) return SimpleResult.Success(null);
      if (raw == NullTerm) return SimpleResult.Success(null);
      if (T == typeof(string)) return SimpleResult.Success(raw);

      if (T.IsPrimitive)
      {
        MethodInfo parse = T.GetMethod(
          "Parse",
          BindingFlags.Public | BindingFlags.Static,
          new Type[] { typeof(string) }
        );

        try
        {
          return SimpleResult.Success(
            parse.Invoke(null, new object[] { raw })
          );
        }
        catch (Exception e)
        {
          return new SimpleResult()
          {
            Ok = false,
            Details = $"-- Parser couldn't parse [{raw}] into primitive type [{T}] because [{e.InnerException?.Message}]",
            Exception = e,
            Result = DefaultFor(T),
          };
        }
      }

      if (T.IsEnum)
      {
        try
        {
          return SimpleResult.Success(
            Enum.Parse(T, raw)
          );
        }
        catch (Exception e)
        {
          return new SimpleResult()
          {
            Ok = false,
            Details = $"-- Parser couldn't parse [{raw}] into Enum [{T}] because [{e.InnerException?.Message}]",
            Exception = e,
            Result = DefaultFor(T),
          };
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
          return new SimpleResult()
          {
            Ok = false,
            Details = $"-- Parser couldn't parse [{raw}] into [{T}] because it doesn't have the Parse method",
            Result = DefaultFor(T),
          };
        }

        try
        {
          return SimpleResult.Success(
            parse.Invoke(null, new object[] { raw })
          );
        }
        catch (Exception e)
        {
          return new SimpleResult()
          {
            Ok = false,
            Details = $"-- Parser couldn't parse [{raw}] into [{T}] because [{e.InnerException?.Message}]",
            Exception = e,
            Result = DefaultFor(T),
          };
        }
      }

      return SimpleResult.Success(
        DefaultFor(T)
      );
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
        //TODO this is cringe
        // if (Verbose) IConfig.Logger.Warning($"-- Parser couldn't serialize object of [{o.GetType()}] type because [{e.Message}]");
      }

      return result;
    }
  }
}
