using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace JovianRadiationRework
{
  /// <summary>
  /// Why??? The problem is that Debug class is supposed to be partial and i can't define it in multiple namespaces
  /// So i'm supposed to copy paste Vitalize logic in every namespace or extract it like here
  /// </summary>
  public static class DebugVitalizer
  {
    public static bool IsDebugAggregator(FieldInfo fi)
      => fi.FieldType.Name.StartsWith("DebugAggregator");
    public static bool IsDebugGate(FieldInfo fi)
      => fi.FieldType.Name.StartsWith("DebugGate");

    public static void VitalizeMe()
      => Vitalize((new StackTrace()).GetFrame(1).GetMethod().DeclaringType);
    public static void Vitalize<DebugType>() => Vitalize(typeof(DebugType));
    public static void Vitalize(Type debugType)
    {
      foreach (FieldInfo fi in debugType.GetFields(BindingFlags.Public | BindingFlags.Static))
      {
        if (IsDebugAggregator(fi))
        {
          (fi.FieldType.GetField("Install").GetValue(fi.GetValue(null)) as Delegate).DynamicInvoke(fi.GetValue(null));
        }
        else if (IsDebugGate(fi))
        {
          if (fi.GetValue(null) is null)
          {
            fi.SetValue(null, Activator.CreateInstance(fi.FieldType));
          }
        }
      }
    }
  }
}