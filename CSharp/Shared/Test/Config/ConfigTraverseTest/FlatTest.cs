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
      public class FlatTest : ConfigTraverseTest
      {
        public override void CreateTests()
        {
          ExampleConfigs.ConfigA TestConfig = new ExampleConfigs.ConfigA();

          Tests.Add(new UListTest(
            TestConfig.GetFlat().Values,
            TestConfig.GetPropsRec()
          ));

          Tests.Add(new UListTest(
            TestConfig.GetFlatValues().Values,
            TestConfig.GetPropsRec().Select(ce => ce.Value)
          ));

          Tests.Add(new UListTest(
            TestConfig.GetFlat().Keys.Select(propPath => TestConfig.Get(propPath).Value),
            TestConfig.GetFlatValues().Values
          ));
        }
      }
    }

  }
}