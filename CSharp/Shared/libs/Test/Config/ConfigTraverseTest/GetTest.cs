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
      public class GetTest : ConfigTraverseTest
      {
        public class InvalidGetTest : GetTest
        {
          public override void CreateTests()
          {
            ExampleConfigs.ConfigA config = new ExampleConfigs.ConfigA();
            IConfig ic = config;

            Tests.Add(new UTest(null as object, null));
            Tests.Add(new UTest(config.Get("").Value, null));
            Tests.Add(new UTest(config.Get("NestedConfigB").Get(null).Value, null));
            Tests.Add(new UTest(config.Get("NestedConfigB", null).Value, null));
            Tests.Add(new UTest(config.Get(null, null).Value, null));
            Tests.Add(new UTest(config.Get(null).Get(null).Value, null));
            Tests.Add(new UTest(config.Get(".").Value, null));
            Tests.Add(new UTest(config.Get("NestedConfigB.213.FloatProp").Value, null));
            Tests.Add(new UTest(config.Get("NestedConfigB..FloatProp").Value, null));
            Tests.Add(new UTest(config.Get(".NestedConfigC.FloatProp").Value, null));
          }
        }

        public override void CreateTests()
        {
          ExampleConfigs.ConfigA config = new ExampleConfigs.ConfigA();

          // extension get method
          Tests.Add(new UTest(config.Get("IntProp").Value, config.IntProp));
          Tests.Add(new UTest(config.Get("FloatProp").Value, config.FloatProp));
          Tests.Add(new UTest(config.Get("StringProp").Value, config.StringProp));
          Tests.Add(new UTest(config.Get("ShouldNotBeDugInto").Value, config.ShouldNotBeDugInto));
          Tests.Add(new UTest(config.Get("NestedConfigB").Value, config.NestedConfigB));

          Tests.Add(new UTest(config.Get("NestedConfigB", "IntProp").Value, config.NestedConfigB.IntProp));
          Tests.Add(new UTest(config.Get("NestedConfigB", "FloatProp").Value, config.NestedConfigB.FloatProp));

          Tests.Add(new UTest(config.Get("NestedConfigB").Get("IntProp").Value, config.NestedConfigB.IntProp));
          Tests.Add(new UTest(config.Get("NestedConfigB").Get("FloatProp").Value, config.NestedConfigB.FloatProp));
          Tests.Add(new UTest(config.Get("NestedConfigB").Get("NestedConfigC").Value, config.NestedConfigB.NestedConfigC));
          Tests.Add(new UTest(config.Get("NestedConfigB").Get("NestedConfigC").Get("FloatProp").Value, config.NestedConfigB.NestedConfigC.FloatProp));

          Tests.Add(new UTest(config.NestedConfigB.Get("IntProp").Value, config.NestedConfigB.IntProp));

          IConfig ic = config;

          // interface get method
          Tests.Add(new UTest(ic.Get("IntProp").Value, config.IntProp));
          Tests.Add(new UTest(ic.Get("FloatProp").Value, config.FloatProp));
          Tests.Add(new UTest(ic.Get("StringProp").Value, config.StringProp));
          Tests.Add(new UTest(ic.Get("ShouldNotBeDugInto").Value, config.ShouldNotBeDugInto));
          Tests.Add(new UTest(ic.Get("NestedConfigB").Value, config.NestedConfigB));

          Tests.Add(new UTest(ic.Get("NestedConfigB", "IntProp").Value, config.NestedConfigB.IntProp));
          Tests.Add(new UTest(ic.Get("NestedConfigB", "FloatProp").Value, config.NestedConfigB.FloatProp));

          Tests.Add(new UTest(ic.Get("NestedConfigB").Get("IntProp").Value, config.NestedConfigB.IntProp));
          Tests.Add(new UTest(ic.Get("NestedConfigB").Get("FloatProp").Value, config.NestedConfigB.FloatProp));
          Tests.Add(new UTest(ic.Get("NestedConfigB").Get("NestedConfigC").Value, config.NestedConfigB.NestedConfigC));
          Tests.Add(new UTest(ic.Get("NestedConfigB").Get("NestedConfigC").Get("FloatProp").Value, config.NestedConfigB.NestedConfigC.FloatProp));

          // interface indexer
          Tests.Add(new UTest(ic["IntProp"].Value, config.IntProp));
          Tests.Add(new UTest(ic["FloatProp"].Value, config.FloatProp));
          Tests.Add(new UTest(ic["StringProp"].Value, config.StringProp));
          Tests.Add(new UTest(ic["ShouldNotBeDugInto"].Value, config.ShouldNotBeDugInto));
          Tests.Add(new UTest(ic["NestedConfigB"].Value, config.NestedConfigB));

          Tests.Add(new UTest(ic["NestedConfigB"]["IntProp"].Value, config.NestedConfigB.IntProp));
          Tests.Add(new UTest(ic["NestedConfigB"]["FloatProp"].Value, config.NestedConfigB.FloatProp));

          Tests.Add(new UTest(ic["NestedConfigB"]["IntProp"].Value, config.NestedConfigB.IntProp));
          Tests.Add(new UTest(ic["NestedConfigB"]["FloatProp"].Value, config.NestedConfigB.FloatProp));
          Tests.Add(new UTest(ic["NestedConfigB"]["NestedConfigC"].Value, config.NestedConfigB.NestedConfigC));
          Tests.Add(new UTest(ic["NestedConfigB"]["NestedConfigC"].Get("FloatProp").Value, config.NestedConfigB.NestedConfigC.FloatProp));

          // Flat paths 
          Tests.Add(new UTest(config.Get("NestedConfigB").Get("NestedConfigC").Get("FloatProp").Value, config.NestedConfigB.NestedConfigC.FloatProp));
          Tests.Add(new UTest(config.Get("NestedConfigB.NestedConfigC").Get("FloatProp").Value, config.NestedConfigB.NestedConfigC.FloatProp));
          Tests.Add(new UTest(config.Get("NestedConfigB").Get("NestedConfigC.FloatProp").Value, config.NestedConfigB.NestedConfigC.FloatProp));
          Tests.Add(new UTest(config.Get("NestedConfigB.NestedConfigC.FloatProp").Value, config.NestedConfigB.NestedConfigC.FloatProp));
        }
      }
    }

  }
}