using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Xml.Linq;
namespace BaroJunk
{

  public interface IConfigEntry
  {
    public string Name { get; }
    public Type Type { get; }
    public object Value { get; set; }
    public IConfigEntry Get(string entryPath);
    public IEnumerable<IConfigEntry> Entries { get; }
    public bool IsConfig { get; }
    public bool IsValid { get; }
  }



}