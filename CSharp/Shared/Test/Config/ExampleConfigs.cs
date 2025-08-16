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
    public partial class ExampleConfigs
    {
      public class EmptyConfig : IConfig { }

      public class ShouldNotBeDugInto
      {
        public int IntProp { get; set; } = 0;
        public float FloatProp { get; set; } = 1.0f;

        public override bool Equals(object obj) => obj is ShouldNotBeDugInto;
        public override string ToString() => "ShouldNotBeDugInto";
      }

      public class ConfigA : IConfig
      {
        public int IntProp { get; set; } = 2;
        public float FloatProp { get; set; } = 3.0f;
        public string StringProp { get; set; } = "bruh";
        public string NullStringProp { get; set; }
        public ShouldNotBeDugInto ShouldNotBeDugInto { get; set; } = new();
        public ConfigB NestedConfigB { get; set; } = new();
        public ConfigB NestedNullConfigB { get; set; }
        public EmptyConfig EmptyConfig { get; set; } = new();
      }

      public class ConfigB : IConfig
      {
        public int IntProp { get; set; } = 4;
        public float FloatProp { get; set; } = 5.0f;
        public string StringProp { get; set; } = "bruh";
        public string NullStringProp { get; set; }
        public ConfigC NestedConfigC { get; set; } = new();
        public ConfigC NestedNullConfigC { get; set; }
        public EmptyConfig EmptyConfig { get; set; } = new();
      }

      public class ConfigC : IConfig
      {
        public int IntProp { get; set; } = 6;
        public float FloatProp { get; set; } = 7.0f;
        public string StringProp { get; set; } = "bruh";
        public string NullStringProp { get; set; }
      }
    }
  }
}