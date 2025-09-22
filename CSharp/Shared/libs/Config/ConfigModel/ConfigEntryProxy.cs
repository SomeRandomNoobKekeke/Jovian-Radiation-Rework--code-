using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;

using Barotrauma;

namespace BaroJunk
{
  public class ConfigEntryProxy : IConfigEntry
  {
    public ConfigModel Model;
    public ConfigEntry Entry;
    public string Path;

    public IConfigEntry Get(string entryPath)
      => Model.Get(string.Join('.', Path, entryPath));
    public IEnumerable<IConfigEntry> Entries
      => Entry.Entries.Select(
        Entry => Model.Get(string.Join('.', Path, Entry.Name))
      );
    public object Value
    {
      get => Entry.Value;
      set
      {
        Entry.Value = value;
        Model.RaiseOnPropChanged(Path, value);
      }
    }
    public bool IsConfig => Entry.IsConfig;
    public bool IsValid => Entry.IsValid;
    public string Name => Entry.Name;
    public Type Type => Entry.Type;

    public ConfigEntryProxy(ConfigModel model, string path, ConfigEntry entry) => (Model, Path, Entry) = (model, path, entry);

    public override string ToString() => Entry.ToString();
  }

}