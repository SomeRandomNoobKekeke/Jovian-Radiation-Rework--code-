using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO;

using Barotrauma;
using ResuscitationKit;

namespace JovianRadiationRework
{
  public partial class ConfigTest : UTestPack
  {
    public class UseCasesTest : ConfigTest
    {
      public override void CreateTests()
      {
        // try
        // {
        //   ExampleConfigs.ConfigA config = new ExampleConfigs.ConfigA();
        //   Mod.Log(config.Get("IntProp", "bebebe", "jujuju"));

        //   config.IntProp = 2;
        //   Tests.Add(new UTest(config.Get("IntProp").Value, config.IntProp));
        // }
        // catch (Exception e) { Mod.Log(e); }


      }
    }


  }
}