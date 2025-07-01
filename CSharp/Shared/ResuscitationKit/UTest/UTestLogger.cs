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
    public static Color SuccessColor = Color.Lime;
    public static Color FailureColor = Color.Red;
    public static string InnerTextColor = "128,255,255";
    public static float ColorContrast = 0.75f;
    public static string Line = "-------------------------------------------------------------";
    public static Color StateColor(bool state) => state ?
      Color.Lerp(Color.White, SuccessColor, ColorContrast) :
      Color.Lerp(Color.White, FailureColor, ColorContrast);

    public static string WrapInColor(object msg, string color)
      => $"‖color:{color}‖{msg}‖end‖";

    public static string AsText(object msg) => WrapInColor(msg, InnerTextColor);

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