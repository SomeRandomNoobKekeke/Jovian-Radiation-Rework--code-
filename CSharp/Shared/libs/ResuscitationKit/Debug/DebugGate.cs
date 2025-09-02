using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
namespace JovianRadiationRework
{

  public class DebugGate
  {
    public IDebugSwitch Switch { get; set; } = new DebugSwitch();
    public Func<bool> Condition { get; set; }
    public event Action OnCapture;

    public void Capture()
    {
      if (Switch.State && (Condition is null || Condition.Invoke())) OnCapture?.Invoke();
    }
  }

  public class DebugGate<T1>
  {
    public IDebugSwitch Switch { get; set; } = new DebugSwitch();
    public Func<T1, bool> Condition { get; set; }
    public event Action<T1> OnCapture;

    public void Capture(T1 arg1)
    {
      Mod.Log(Switch.State);
      if (Switch.State && (Condition is null || Condition.Invoke(arg1))) OnCapture?.Invoke(arg1);
    }
  }

  public class DebugGate<T1, T2>
  {
    public IDebugSwitch Switch { get; set; } = new DebugSwitch();
    public Func<T1, T2, bool> Condition { get; set; }
    public event Action<T1, T2> OnCapture;

    public void Capture(T1 arg1, T2 arg2)
    {
      if (Switch.State && (Condition is null || Condition.Invoke(arg1, arg2))) OnCapture?.Invoke(arg1, arg2);
    }
  }




}