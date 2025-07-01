using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Text;

namespace ResuscitationKit
{
  public class UTestPack
  {
    public static bool IsTestGenerator(MethodInfo mi)
      => mi.ReturnType == typeof(UTest) && mi.GetParameters().Length == 0;

    public static string GetNameFromMethodInfo(MethodInfo mi)
      => $"{mi.DeclaringType.Name}.{mi.Name}";

    public static UTestPack Run(string name)
    {
      if (!UTestExplorer.TestByName.ContainsKey(name))
      {
        UTestLogger.Warning($"No such test");
        return null;
      }

      return Run(UTestExplorer.TestByName[name]);
    }
    public static UTestPack Run<T>() => Run(typeof(T));
    public static UTestPack Run(Type T)
    {
      if (!T.IsAssignableTo(typeof(UTestPack)))
        throw new ArgumentException($"[{T}] is not a UTestPack");

      UTestPack pack = Activator.CreateInstance(T) as UTestPack;

      foreach (UTest test in pack.Tests) test?.Run();

      return pack;
    }

    //FIXME i'm cringe
    public static void RunRecursive<T>() => RunRecursive(typeof(T));
    public static void RunRecursive(Type T = null)
    {
      T ??= typeof(UTestPack);

      UTestExplorer.TestTree.RunRecursive((test) =>
      {
        if (test == typeof(UTestPack)) return; // bruh
        UTestPack.Run(test).Log();
      }, T);
    }

    public List<UTest> Tests = new();
    public int PassedCount => Tests.Count(test => test.State);

    public virtual void CreateTests()
    {
      foreach (MethodInfo mi in this.GetType().GetMethods())
      {
        if (mi.DeclaringType == this.GetType() && IsTestGenerator(mi))
        {
          UTest test = mi.Invoke(mi.IsStatic ? null : this, null) as UTest;
          test.Name ??= GetNameFromMethodInfo(mi);
          Tests.Add(test);
        }
      }
    }

    public UTestPack() { CreateTests(); }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();

      sb.Append($"UTestPack {this.GetType()} [{PassedCount}/{Tests.Count}]:\n");
      foreach (UTest test in Tests)
      {
        sb.Append(test.ToString());
        sb.Append('\n');
      }
      return sb.ToString();
    }

    public void Log() => UTestLogger.LogPack(this);
  }
}