using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
namespace ResuscitationKit
{
  public class DebugAggregator : DebugEvent
  {
    public Action<DebugAggregator> Install;
    protected override DebugStateContainer state { get; set; } = new DebugStateAlwaysOn();
    public DebugAggregator(Action<DebugAggregator> install) => Install = install;
  }

  public class DebugAggregator<T1> : DebugEvent<T1>
  {
    public Action<DebugAggregator<T1>> Install;
    protected override DebugStateContainer state { get; set; } = new DebugStateAlwaysOn();
    public DebugAggregator(Action<DebugAggregator<T1>> install) => Install = install;
  }
  public class DebugAggregator<T1, T2> : DebugEvent<T1, T2>
  {
    public Action<DebugAggregator<T1, T2>> Install;
    protected override DebugStateContainer state { get; set; } = new DebugStateAlwaysOn();
    public DebugAggregator(Action<DebugAggregator<T1, T2>> install) => Install = install;
  }
  public class DebugAggregator<T1, T2, T3> : DebugEvent<T1, T2, T3>
  {
    public Action<DebugAggregator<T1, T2, T3>> Install;
    protected override DebugStateContainer state { get; set; } = new DebugStateAlwaysOn();
    public DebugAggregator(Action<DebugAggregator<T1, T2, T3>> install) => Install = install;
  }
  public class DebugAggregator<T1, T2, T3, T4> : DebugEvent<T1, T2, T3, T4>
  {
    public Action<DebugAggregator<T1, T2, T3, T4>> Install;
    protected override DebugStateContainer state { get; set; } = new DebugStateAlwaysOn();
    public DebugAggregator(Action<DebugAggregator<T1, T2, T3, T4>> install) => Install = install;
  }
  public class DebugAggregator<T1, T2, T3, T4, T5> : DebugEvent<T1, T2, T3, T4, T5>
  {
    public Action<DebugAggregator<T1, T2, T3, T4, T5>> Install;
    protected override DebugStateContainer state { get; set; } = new DebugStateAlwaysOn();
    public DebugAggregator(Action<DebugAggregator<T1, T2, T3, T4, T5>> install) => Install = install;
  }
}