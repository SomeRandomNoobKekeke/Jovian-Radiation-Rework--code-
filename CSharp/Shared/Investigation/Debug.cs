using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

namespace JovianRadiationRework
{
  public class DebugAggregator
  {
    public bool Active { get; set; }
    public Action<string, object[]> Action { get; set; }
    public void Capture(string caption, params object[] args)
    {
      if (!Active) return;
      Action?.Invoke(caption, args);
    }
  }

  public static class RadiationDebug
  {
    public static DebugAggregator bruh = new()
    {
      Active = true,
      Action = (caption, args) => Mod.Log($"{caption}: [{String.Join(", ", args.Select(o => o?.ToString()))}]"),
    };
  }
}