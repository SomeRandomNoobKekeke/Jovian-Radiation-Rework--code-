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

        ConfigSaver.Use(config, Path.Combine(ConfigSaver.ModDir<Mod>(), "Ignore", "ConfigSaverTest.xml"));

        config.IntProp = 101;

        ConfigSaver.Save();
        config.IntProp = 202;
        ConfigSaver.Load();

        Tests.Add(new UTest(config.IntProp, 101));

        ConfigSaver.Use(config);
        config.IntProp = 55;
        ConfigSaver.Save();
        config.IntProp = 66;
        ConfigSaver.Load();

        Tests.Add(new UTest(config.IntProp, 55));
      }
    }

  }
}