using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace BaroJunk
{
  /// <summary>
  /// Wrapper around an object that adds Get Set methods
  /// Accepts strings and parses them automatically
  /// Made for console commands
  /// </summary>
  public class ReflectionProxy
  {
    static ReflectionProxy() => ProjectInfo.Add(new PackageInfo()
    {
      Name = "ReflectionProxy",
      Version = new Version(0, 0, 0)
      {
        Branch = "BaroJunk"
      }
    });


    public class NotAProp : Attribute { }

    public static BindingFlags pls = BindingFlags.Instance | BindingFlags.Public;
    public object Target { get; set; }

    public object Get(string prop) => GetInfo(prop)?.GetValue(Target);

    public SimpleResult Set(string prop, string raw)
    {
      if (!Has(prop)) return SimpleResult.Failure("no such prop");
      SimpleResult result = SimpleParser.Default.Parse(raw, TypeOf(prop));
      if (result.Ok) GetInfo(prop)?.SetValue(Target, result.Result);
      return result;
    }

    public bool Has(string prop) => GetInfo(prop) is not null;

    public Type TypeOf(string prop)
    {
      PropertyInfo pi = GetInfo(prop);
      if (pi is null) return null;
      return pi.PropertyType;
    }

    public IEnumerable<string> GetPropNames()
      => Target.GetType().GetProperties(pls)
      .Where(pi => !Attribute.IsDefined(pi, typeof(NotAProp)))
      .Select(pi => pi.Name);

    public PropertyInfo GetInfo(string prop)
    {
      PropertyInfo pi = Target.GetType().GetProperty(prop, pls);
      if (pi is null) return null;
      if (Attribute.IsDefined(pi, typeof(NotAProp))) return null;
      return pi;
    }

    public ReflectionProxy(object target)
    {
      ArgumentNullException.ThrowIfNull(target);
      Target = target;
    }

    public override string ToString() => Target.ToString();
  }

}
