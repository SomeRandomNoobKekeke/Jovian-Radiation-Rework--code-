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

namespace BaroJunk_Config
{
  /// <summary>
  /// It's just an object where you can listen for reactive events
  /// </summary>
  public class ReactiveCore
  {
    public ConfigCore Core { get; }
    public IConfiglike Host => Core.Host;
    public ReactiveEntryLocator Locator { get; }

    public event Action<string, object> PropChanged;
    public event Action Updated;

    public Action<string, object> OnPropChanged { set { PropChanged += value; } }
    public Action OnUpdated { set { Updated += value; } }

    public void RaisePropChanged(string key, object value) => PropChanged?.Invoke(key, value);
    public void RaiseUpdated() => Updated?.Invoke();

    public ReactiveCore(ConfigCore core)
    {
      Core = core;
      Locator = new ReactiveEntryLocator(this, new IConfigLikeLocatorAdapter(Host), null);
    }

    public override string ToString() => $"ReactiveCore [{GetHashCode()}]";
  }

}