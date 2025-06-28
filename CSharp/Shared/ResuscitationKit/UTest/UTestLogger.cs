using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using Barotrauma;
using Microsoft.Xna.Framework;

namespace ResuscitationKit
{
  public class UTestLogger
  {
    public static string Line = "-------------------------------------------------------------";
    public static Color StateColor(bool state) => state ? Color.Lime : Color.Red;

    public static string WrapInColor(object msg, string color)
      => $"‖color:{color}‖{msg}‖end‖";

    public static void Log(object msg, Color? cl = null)
    {
      cl ??= Color.Cyan;
      LuaCsLogger.LogMessage($"{msg ?? "null"}", cl * 0.8f, cl);
    }

    public static void Warning(object msg) => Log(msg, Color.Yellow);

    public static void LogTest(UTest test)
    {
      Log(test, StateColor(test.State));
    }

    public static void LogPack(UTestPack pack)
    {
      Log(Line);
      Log($"UTestPack {pack.GetType()} [{pack.PassedCount}/{pack.Tests.Count}]:");

      foreach (UTest test in pack.Tests)
      {
        test.Log();
      }
    }
  }
}