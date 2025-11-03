using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using System.Text;

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace BaroJunk
{

  public partial class Logger
  {
    /// <summary>
    /// Some custom serialization methods,
    /// Tooks them from parser, some of them are garbage
    /// </summary>
    public class Wrap
    {
      public static string ExceptionMessage(Exception e)
        => $"[{e.Message}{(e.InnerException is null ? null : $" - {e.InnerException.Message}")}]";
      public static string IEnumerable(IEnumerable<object> array)
        => $"[{String.Join(", ", array?.Select(o => WrapInColor(o?.ToString(), "white")) ?? new string[] { })}]";

      public static string IDictionary(System.Collections.IDictionary dict)
      {
        StringBuilder sb = new StringBuilder();

        sb.Append("{\n");
        foreach (System.Collections.DictionaryEntry entry in dict)
        {
          sb.Append($"    {entry.Key}: [{WrapInColor(entry.Value, "white")}],\n");
        }
        sb.Append("} ");

        return sb.ToString();
      }

      /// <summary>
      /// beware of loops
      /// </summary>
      public static string Object(object target)
      {
        StringBuilder sb = new StringBuilder();

        //BRUH
        bool isPrimitive(PropertyInfo pi)
          => pi.PropertyType.IsPrimitive || pi.PropertyType == typeof(string);

        void ToStringRec(string offset, object o)
        {
          Logger.Default.Log($"Wrapping [{o}]");
          if (o is null)
          {
            sb.Append("[null]");
            return;
          }

          PropertyInfo[] props = o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

          foreach (PropertyInfo pi in props)
          {
            if (!isPrimitive(pi))
            {
              object value = pi.GetValue(o);

              sb.Append($"{offset}{pi.PropertyType.Name}  {pi.Name}:\n");
              ToStringRec($"{offset}       |", value);
              sb.Append($"{offset}        \n");
            }
          }

          foreach (PropertyInfo pi in props)
          {
            if (isPrimitive(pi))
            {
              sb.Append($"{offset}{pi.PropertyType.Name}  {pi.Name}: [{WrapInColor(pi.GetValue(o), "white")}]\n");
            }
          }
        }

        ToStringRec("", target);
        sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
      }

      public static string AsJson(object target)
      {
        return JsonSerializer.Serialize(target, new JsonSerializerOptions
        {
          WriteIndented = true,
          Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
      }

      /// <summary>
      /// Just direct props of an object
      /// </summary>
      public static string Props(object target)
      {
        if (target is null) return "[null]";
        StringBuilder sb = new StringBuilder();
        foreach (PropertyInfo pi in target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
          sb.Append($"{pi.PropertyType.Name}  {pi.Name}: [{WrapInColor(pi.GetValue(target), "white")}]\n");
        }

        return sb.ToString();
      }


    }
  }

}