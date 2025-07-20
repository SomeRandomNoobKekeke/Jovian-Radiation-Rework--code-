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
      public class BreadthTest : ConfigTraverseTest
      {
        public override void CreateTests()
        {
          List<ConfigEntry> entries = ConfigTraverse.Simple(TestConfig).ToList();

          UTest countTest = new UTest(entries.Count, 13) { DetailsOnFail = entries.ToText("\n", "\n"), };

          Tests.Add(countTest);

          if (!countTest.State) return;

          Tests.Add(new USetTest(
            entries.Slice(0, 4),
            new HashSet<object>()
            {
              new ConfigEntry(TestConfig,"IntProp"),
              new ConfigEntry(TestConfig,"IntProp"),
              new ConfigEntry(TestConfig,"IntProp"),
              new ConfigEntry(TestConfig,"IntProp"),
            }
          ));



        }
      }
    }

  }
}