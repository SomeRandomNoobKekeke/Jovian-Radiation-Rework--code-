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
  public class ReflectionStringProxy
  {
    public static BindingFlags pls = BindingFlags.Instance | BindingFlags.Public;
    public object Target { get; set; }

    public object Get(string prop) => Target.GetType().GetProperty(prop, pls)?.GetValue(Target);
    public SimpleResult Set(string prop, string raw)
    {
      if (!Has(prop)) return SimpleResult.Failure("no such prop");
      SimpleResult result = SimpleParser.Default.Parse(raw, TypeOf(prop));
      if (result.Ok)
      {
        Target.GetType().GetProperty(prop, pls)?.SetValue(Target, result.Result);
      }
      return result;
    }
    public bool Has(string prop) => Target.GetType().GetProperty(prop, pls) is not null;
    public Type TypeOf(string prop) => Target.GetType().GetProperty(prop, pls)?.PropertyType;
    public IEnumerable<string> GetProps() => Target.GetType().GetProperties(pls).Select(pi => pi.Name);

    public override string ToString() => Target.ToString();
    public ReflectionStringProxy(object target)
    {
      ArgumentNullException.ThrowIfNull(target);
      Target = target;
    }
  }

}
