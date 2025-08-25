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
    public partial class ConfigSaverTest : ConfigTest
    {
      public override void CreateTests()
      {
        ConfigTest.ExampleConfigs.ConfigA config = new ExampleConfigs.ConfigA();

        config.SetAsCurrent();
        config.SetSavePath(Path.Combine(Utils.ModDir<Mod>(), "Ignore", "ConfigSaverTest.xml"));

        config.IntProp = 101;

        config.Save();
        config.IntProp = 202;
        config.Load();

        Tests.Add(new UTest(config.IntProp, 101));

        config.SetSavePath(null);

        config.IntProp = 55;
        config.Save();
        config.IntProp = 66;
        config.Load();

        Tests.Add(new UTest(config.IntProp, 55));
      }
    }

  }
}