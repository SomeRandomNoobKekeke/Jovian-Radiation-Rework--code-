using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
namespace JovianRadiationRework
{
  public class DebugGateCore
  {
    protected virtual DebugStateContainer state { get; set; } = new DebugStateContainer();
    public bool State { get => state.Value; set => state.Value = value; }
    public void Enable() => State = true;
    public void Disable() => State = false;
    public void Toggle() => State = !State;
  }

  public class DebugGate : DebugGateCore
  {
    public event Action OnCapture;
    public void Raise() { if (State) OnCapture?.Invoke(); }
    public void Add(Action callback) => OnCapture += callback;
    public void Remove(Action callback) => OnCapture -= callback;
  }

  public class DebugGate<T1> : DebugGateCore
  {
    public event Action<T1> OnCapture;
    public void Raise(T1 arg1) { if (State) OnCapture?.Invoke(arg1); }
    public void Add(Action<T1> callback) => OnCapture += callback;
    public void Remove(Action<T1> callback) => OnCapture -= callback;
  }
  public class DebugGate<T1, T2> : DebugGateCore
  {
    public event Action<T1, T2> OnCapture;
    public void Raise(T1 arg1, T2 arg2) { if (State) OnCapture?.Invoke(arg1, arg2); }
    public void Add(Action<T1, T2> callback) => OnCapture += callback;
    public void Remove(Action<T1, T2> callback) => OnCapture -= callback;
  }
  public class DebugGate<T1, T2, T3> : DebugGateCore
  {
    public event Action<T1, T2, T3> OnCapture;
    public void Raise(T1 arg1, T2 arg2, T3 arg3) { if (State) OnCapture?.Invoke(arg1, arg2, arg3); }
    public void Add(Action<T1, T2, T3> callback) => OnCapture += callback;
    public void Remove(Action<T1, T2, T3> callback) => OnCapture -= callback;
  }
  public class DebugGate<T1, T2, T3, T4> : DebugGateCore
  {
    public event Action<T1, T2, T3, T4> OnCapture;
    public void Raise(T1 arg1, T2 arg2, T3 arg3, T4 arg4) { if (State) OnCapture?.Invoke(arg1, arg2, arg3, arg4); }
    public void Add(Action<T1, T2, T3, T4> callback) => OnCapture += callback;
    public void Remove(Action<T1, T2, T3, T4> callback) => OnCapture -= callback;
  }
  public class DebugGate<T1, T2, T3, T4, T5> : DebugGateCore
  {
    public event Action<T1, T2, T3, T4, T5> OnCapture;
    public void Raise(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) { if (State) OnCapture?.Invoke(arg1, arg2, arg3, arg4, arg5); }
    public void Add(Action<T1, T2, T3, T4, T5> callback) => OnCapture += callback;
    public void Remove(Action<T1, T2, T3, T4, T5> callback) => OnCapture -= callback;
  }
}