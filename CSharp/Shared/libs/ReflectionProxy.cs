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
  public class ReflectionProxy
  {
    public static BindingFlags pls = BindingFlags.Instance | BindingFlags.Public;
    public object Target { get; private set; }

    public object Get(string prop) => Target.GetType().GetProperty(prop, pls)?.GetValue(Target);
    public void Set(string prop, object value) => Target.GetType().GetProperty(prop, pls)?.SetValue(Target, value);
    public bool Has(string prop) => Target.GetType().GetProperty(prop, pls) is not null;
    public IEnumerable<string> Props => Target.GetType().GetProperties(pls).Select(pi => pi.Name);

    public override string ToString() => Target.ToString();
    public ReflectionProxy(object target)
    {
      ArgumentNullException.ThrowIfNull(target);
      Target = target;
    }
  }

}
