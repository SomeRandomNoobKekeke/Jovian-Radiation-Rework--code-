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


namespace JovianRadiationRework
{
  public partial class ConfigTest : UTestPack
  {
    public partial class ConfigTraverseTest : ConfigTest
    {
      public class GetPropsRecTest : ConfigTraverseTest
      {
        public override void CreateTests()
        {
          ExampleConfigs.ConfigA TestConfig = new ExampleConfigs.ConfigA();

          List<ConfigEntry> entries = TestConfig.GetPropsRec().ToList();

          UTest countTest = new UTest(entries.Count, 13) { DetailsOnFail = entries.ToText("\n", "\n"), };

          Tests.Add(countTest);

          if (!countTest.Passed) return;

          Tests.Add(new USetTest(
            entries.Slice(0, 5),
            new HashSet<object>()
            {
              new ConfigEntry(TestConfig,"IntProp"),
              new ConfigEntry(TestConfig,"FloatProp"),
              new ConfigEntry(TestConfig,"StringProp"),
              new ConfigEntry(TestConfig,"NullStringProp"),
              new ConfigEntry(TestConfig,"ShouldNotBeDugInto"),
            }
          ));

          Tests.Add(new USetTest(
            entries.Slice(5, 4),
            new HashSet<object>()
            {
              new ConfigEntry(TestConfig.NestedConfigB,"IntProp"),
              new ConfigEntry(TestConfig.NestedConfigB,"FloatProp"),
              new ConfigEntry(TestConfig.NestedConfigB,"StringProp"),
              new ConfigEntry(TestConfig.NestedConfigB,"NullStringProp"),
            }
          ));

          Tests.Add(new USetTest(
            entries.Slice(9, 4),
            new HashSet<object>()
            {
              new ConfigEntry(TestConfig.NestedConfigB.NestedConfigC,"IntProp"),
              new ConfigEntry(TestConfig.NestedConfigB.NestedConfigC,"FloatProp"),
              new ConfigEntry(TestConfig.NestedConfigB.NestedConfigC,"StringProp"),
              new ConfigEntry(TestConfig.NestedConfigB.NestedConfigC,"NullStringProp"),
            }
          ));
        }
      }
    }

  }
}