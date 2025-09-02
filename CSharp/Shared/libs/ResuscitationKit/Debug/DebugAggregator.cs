using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
namespace JovianRadiationRework
{
  public class DebugAggregator : DebugGate
  {
    public Action<DebugAggregator> Install { set { value?.Invoke(this); } }
    public DebugAggregator() => Switch.State = true;
  }

  public class DebugAggregator<T1> : DebugGate<T1>
  {
    public Action<DebugAggregator<T1>> Install { set { value?.Invoke(this); } }
    public DebugAggregator() => Switch.State = true;
  }

  public class DebugAggregator<T1, T2> : DebugGate<T1, T2>
  {
    public Action<DebugAggregator<T1, T2>> Install { set { value?.Invoke(this); } }
    public DebugAggregator() => Switch.State = true;
  }
}