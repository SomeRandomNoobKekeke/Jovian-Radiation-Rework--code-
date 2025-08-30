using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace JovianRadiationRework
{

  public class UTestExplorer
  {
    public static TypeTree TestTree = new TypeTree(typeof(UTestPack));
    public static IEnumerable<string> TestNames
      => TestTree.TypeByPath.Keys;
    public static Dictionary<string, Type> TestByName => TestTree.TypeByPath;

    public static void PrintAllTests()
    {
      UTestLogger.Log($"Available UTests:");
      UTestLogger.Log(TestTree);
    }
  }
}