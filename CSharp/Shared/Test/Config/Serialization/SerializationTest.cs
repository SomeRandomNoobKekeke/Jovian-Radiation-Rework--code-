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
using System.Xml;
using System.Xml.Linq;

namespace JovianRadiationRework
{
  public partial class ConfigTest : UTestPack
  {
    public partial class SerializationTest : ConfigTest
    {
      public override void CreateTests()
      {
        ExampleConfigs.ConfigA config1 = new ExampleConfigs.ConfigA();
        ExampleConfigs.ConfigA config2 = new ExampleConfigs.ConfigA();

        config1.FromXML(config1.ToXML());

        Mod.Log(config1.ToXML());
        Mod.Log(config2.ToXML());

        Tests.Add(new UTest(config1.IsEqual(config2), true));

        Tests.Add(new UThrowTest(() => ConfigSerialization.ToXML(null), new ArgumentNullException("bla bla"), "ToXML"));
        Tests.Add(new UThrowTest(() => ConfigSerialization.FromXML(null, config1.ToXML()), null, "FromXML"));

        Mod.Log(config1.Compare(config2));
      }
    }

  }
}