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

using System.Xml;
using System.Xml.Linq;

namespace JovianRadiationRework
{
  public partial class ConfigTest : UTestPack
  {
    public partial class SerializationTest : ConfigTest
    {
      public partial class ConfigToHintsTest : SerializationTest
      {
        public override void CreateTests()
        {
          ExampleConfigs.ConfigA config = new ExampleConfigs.ConfigA();

          Tests.Add(new UTest(ConfigSerialization.ToAdvancedHints(config)["NestedConfigB"]["NestedConfigC"].Name, "NestedConfigC"));
        }
      }
    }

  }
}