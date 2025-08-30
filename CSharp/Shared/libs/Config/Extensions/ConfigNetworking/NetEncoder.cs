using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Barotrauma.Networking;

namespace JovianRadiationRework
{
  public static partial class NetEncoder
  {
    public static void Encode(IWriteMessage msg, object config)
    {
      foreach (ConfigEntry entry in ConfigTraverse.GetFlat(config).Values)
      {
        NetParser.Encode(msg, entry);
      }
    }

    public static void Decode(IReadMessage msg, object config)
    {
      var flat = ConfigTraverse.GetFlat(config);
      foreach (string key in flat.Keys)
      {
        ConfigEntry entry = flat[key];
        entry.Value = NetParser.Decode(msg, entry.Property.PropertyType);
      }
    }
  }
}