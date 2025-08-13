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
    public partial class ConfigTraverseTest : ConfigTest
    {
      public class GetTestTest : ConfigTraverseTest
      {
        public override void CreateTests()
        {
          ExampleConfigs.ConfigA config = new ExampleConfigs.ConfigA();


          Tests.Add(new UTest(config.Get("FloatProp"), config.IntProp));

          Mod.Log($"{Tests[0].Result.Result.GetType()} {Tests[0].Expected.Result.GetType()}");
        }
      }
    }

  }
}