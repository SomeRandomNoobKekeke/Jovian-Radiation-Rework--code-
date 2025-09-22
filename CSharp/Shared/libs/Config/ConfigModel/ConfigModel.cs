using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace BaroJunk
{
  public class ConfigModel : IConfigEntry
  {
    public IConfig Config;
    public Dictionary<string, ConfigEntryProxy> Proxies = new();

    public event Action<string, object> PropChanged;
    public void RaiseOnPropChanged(string key, object value) => PropChanged?.Invoke(key, value);
    public void OnPropChanged(Action<string, object> action) => PropChanged += action;


    public object Value { get => this; set {/*bruh*/ } }
    public IConfigEntry Get(string entryPath) => Proxies.ContainsKey(entryPath) ? Proxies[entryPath] : ConfigEntry.Empty;
    public IEnumerable<IConfigEntry> Entries
      => EntryAccess.GetAllEntries(Config).Select(Entry => Get(Entry.Name));
    public bool IsConfig => Config.IsConfig;
    public bool IsValid => Config.IsValid;
    public string Name => Config.Name;
    public Type Type => Config.Type;



    public ConfigModel(IConfig config)
    {
      Config = config;
      Dictionary<string, ConfigEntry> flat = EntryAccess.GetAllFlat(config);
      Proxies = flat.ToDictionary(
        kvp => kvp.Key,
        kvp => new ConfigEntryProxy(this, kvp.Key, kvp.Value)
      );
    }
  }



}