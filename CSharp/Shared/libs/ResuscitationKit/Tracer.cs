using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using Barotrauma;
using Microsoft.Xna.Framework;
using System.IO;

namespace BaroJunk
{
  /// <summary>
  /// Helper class to do something on n-th call of a function
  /// </summary>
  public class Tracer
  {
    private Dictionary<string, int> Traced = new();

    /// <summary>
    /// Do this first
    /// </summary>
    /// <param name="key"> tracer id </param>
    /// <param name="i"> n-th call </param>
    public void AddTracer(string key, int i = 1) => Traced[key] = i;

    /// <summary>
    /// Will return true only on n-th call
    /// </summary>
    /// <param name="key"> tracer id </param>
    public bool Trace(string key)
    {
      if (Traced.ContainsKey(key))
      {
        Traced[key]--;
        if (Traced[key] <= 0) Traced.Remove(key);
        return true;
      }
      return false;
    }
  }
}
