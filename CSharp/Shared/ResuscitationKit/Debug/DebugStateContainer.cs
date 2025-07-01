using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;


namespace ResuscitationKit
{
  public class DebugStateContainer
  {
    public virtual bool Value { get; set; }
  }

  public class DebugStateAlwaysOn : DebugStateContainer
  {
    public override bool Value { get => true; set { } }
  }

  public class DebugStateProxy : DebugStateContainer
  {
    public override bool Value
    {
      get => Getter?.Invoke() ?? false;
      set { if (Setter is not null) Setter.Invoke(value); }
    }
    public Func<bool> Getter;
    public Action<bool> Setter;
  }
}