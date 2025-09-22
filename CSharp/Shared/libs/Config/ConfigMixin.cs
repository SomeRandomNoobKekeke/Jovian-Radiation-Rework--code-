using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Runtime.CompilerServices;

namespace BaroJunk
{
  /// <summary>
  /// Additional state attached to IConfig
  /// </summary>
  public class ConfigMixin
  {
    public static ConditionalWeakTable<IConfig, ConfigMixin> Mixins = new();

    public IConfig Config;

    public ConfigModel Model;
    public ConfigManager ConfigManager;
    public ConfigLogger Logger;
    public ConfigSettings Settings;
    public IConfigFacades Facades;


    public ConfigMixin(IConfig config)
    {
      Config = config;
      Model = new ConfigModel(config);
      ConfigManager = new ConfigManager(config);
      Settings = new ConfigSettings(config);
      Logger = new ConfigLogger();
      Facades = new ConfigFacades();
    }
  }




}