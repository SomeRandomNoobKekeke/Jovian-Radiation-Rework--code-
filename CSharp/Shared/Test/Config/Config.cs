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
  public class ConfigTest : UTestPack
  {
    public class ConfigA : IConfig
    {
      public int PropA { get; set; }
      public float PropB { get; set; }
      public ConfigB NestedConfig { get; set; } = new ConfigB();
    }

    public class ConfigB : IConfig
    {
      public int PropA { get; set; }
      public float PropB { get; set; }
    }


    public class BreadthTest : ConfigTest
    {
      public override void CreateTests()
      {
        ConfigA config = new ConfigA();
        List<ConfigEntry> entries = ConfigTraverse.Breadth(config).ToList();

        Tests.Add(new UTest(entries.Count, 4));

        Tests.Add(new UTest(entries[0], new ConfigEntry(typeof(ConfigA).GetProperty("PropA"), config)));
        Tests.Add(new UTest(entries[1], new ConfigEntry(typeof(ConfigA).GetProperty("PropB"), config)));
        Tests.Add(new UTest(entries[2], new ConfigEntry(typeof(ConfigB).GetProperty("PropA"), config.NestedConfig)));
        Tests.Add(new UTest(entries[3], new ConfigEntry(typeof(ConfigB).GetProperty("PropB"), config.NestedConfig)));
      }
    }

    public class DepthTest : ConfigTest
    {
      public override void CreateTests()
      {
        ConfigA config = new ConfigA();
        List<ConfigEntry> entries = ConfigTraverse.Breadth(config).ToList();

        Tests.Add(new UTest(entries.Count, 4));

        Tests.Add(new UTest(entries[0], new ConfigEntry(typeof(ConfigB).GetProperty("PropA"), config.NestedConfig)));
        Tests.Add(new UTest(entries[1], new ConfigEntry(typeof(ConfigB).GetProperty("PropB"), config.NestedConfig)));
        Tests.Add(new UTest(entries[2], new ConfigEntry(typeof(ConfigA).GetProperty("PropA"), config)));
        Tests.Add(new UTest(entries[3], new ConfigEntry(typeof(ConfigA).GetProperty("PropB"), config)));
      }
    }



  }
}