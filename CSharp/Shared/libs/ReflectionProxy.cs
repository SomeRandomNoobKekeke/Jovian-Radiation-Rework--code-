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
  public static class StaticReflectionProxy
  {
    public static BindingFlags pls = BindingFlags.Instance | BindingFlags.Public;

    public static object Get(object target, string prop)
      => target?.GetType().GetProperty(prop, pls)?.GetValue(target);
    public static void Set(object target, string prop, object value)
      => target?.GetType().GetProperty(prop, pls)?.SetValue(target, value);
    public static bool Has(object target, string prop)
      => target?.GetType().GetProperty(prop, pls) is not null;
    public static IEnumerable<string> Props(object target) => target?.GetType().GetProperties(pls).Select(pi => pi.Name);
  }

  public class ReflectionProxy
  {
    public static BindingFlags pls = BindingFlags.Instance | BindingFlags.Public;
    public object Target { get; private set; }
    public object Get(string prop) => Target.GetType().GetProperty(prop, pls)?.GetValue(Target);
    public void Set(string prop, object value) => Target.GetType().GetProperty(prop, pls)?.SetValue(Target, value);
    public bool Has(string prop) => Target.GetType().GetProperty(prop, pls) is not null;
    public IEnumerable<string> Props() => Target.GetType().GetProperties(pls).Select(pi => pi.Name);
    public object this[string prop]
    {
      get => Get(prop);
      set => Set(prop, value);
    }
    public ReflectionProxy(object target)
    {
      ArgumentNullException.ThrowIfNull(target);
      Target = target;
    }
  }
}
