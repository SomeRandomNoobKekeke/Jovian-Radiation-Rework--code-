using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace ResuscitationKit
{

  public class UTestExplorer
  {
    public static TypeTree TestTree = new TypeTree(typeof(UTestPack));
    public static IEnumerable<string> TestNames
      => TestTree.RootNode.DeepChildren.Select(node => node.Type.Name);
    public static Dictionary<string, Type> TestByName => TestTree.TypeByName;

    public static void PrintAllTests()
    {
      UTestLogger.Log($"Available UTests:");
      UTestLogger.Log(TestTree);
    }
  }
}