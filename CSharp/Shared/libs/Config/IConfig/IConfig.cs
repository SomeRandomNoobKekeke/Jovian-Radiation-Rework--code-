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

  public partial interface IConfig : IConfigEntry
  {
    public static string TypeID<T>() => TypeID(typeof(T));
    public static string TypeID(Type T) => $"{T.Namespace}_{T.Name}";
    public string ID => TypeID(this.GetType());

    public ConfigMixin Mixin => ConfigMixin.Mixins.GetValue(this, c => new ConfigMixin(c));


    public ConfigManager Manager
    {
      get => Mixin.ConfigManager;
      set => Mixin.ConfigManager = value;
    }

    public IConfigFacades Facades
    {
      get => Mixin.Facades;
      set => Mixin.Facades = value;
    }

    //TODO logger probably shouldn't be exposed like this, it should be behind some facade, i'm unable to test log output because of this
    public ConfigLogger Logger
    {
      get => Mixin.Logger;
      set => Mixin.Logger = value;
    }

    public ConfigSettings Settings
    {
      get => Mixin.Settings;
      set => Mixin.Settings = value;
    }

    public void OnPropChanged(Action<string, object> action) => Mixin.Model.OnPropChanged(action);
    public void OnConfigUpdated(Action action) => Mixin.Model.OnConfigUpdated(action);

    //TODO where should it be?
    public IEnumerable<IConfig> SubConfigs
      => EntryAccess.GetAllEntriesRec(this)
      .Where(entry => entry.IsConfig)
      .Select(entry => entry.Value as IConfig);


    object IConfigEntry.Value { get => this; set { /*bruh*/ } }
    IConfigEntry IConfigEntry.Get(string entryPath) => Mixin.Model.Get(entryPath);
    IEnumerable<IConfigEntry> IConfigEntry.Entries => Mixin.Model.Entries;
    bool IConfigEntry.IsConfig => true;
    bool IConfigEntry.IsValid => true; // :BaroDev:
    string IConfigEntry.Name => this.GetType().Name;
    Type IConfigEntry.Type => this.GetType();




    public object GetProp(string propPath) => EntryAccess.GetProp(this, propPath);
    public void SetProp(string propPath, object value) => EntryAccess.SetProp(this, propPath, value);
    public ConfigEntry GetEntry(string propPath) => EntryAccess.GetEntry(this, propPath);
    public IEnumerable<ConfigEntry> GetEntries() => EntryAccess.GetEntries(this);
    public IEnumerable<ConfigEntry> GetAllEntries() => EntryAccess.GetAllEntries(this);
    public IEnumerable<ConfigEntry> GetEntriesRec() => EntryAccess.GetEntriesRec(this);
    public IEnumerable<ConfigEntry> GetAllEntriesRec() => EntryAccess.GetAllEntriesRec(this);
    public Dictionary<string, ConfigEntry> GetFlat() => EntryAccess.GetFlat(this);
    public Dictionary<string, ConfigEntry> GetAllFlat() => EntryAccess.GetAllFlat(this);
    public Dictionary<string, object> GetFlatValues() => EntryAccess.GetFlatValues(this);
    public Dictionary<string, object> GetAllFlatValues() => EntryAccess.GetAllFlatValues(this);


  }



}