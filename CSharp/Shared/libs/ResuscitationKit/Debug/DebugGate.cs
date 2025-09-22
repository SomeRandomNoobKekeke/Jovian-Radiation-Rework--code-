using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
namespace BaroJunk
{

  public class DebugGate
  {
    public bool State { get; set; }
    public Func<bool> Condition { get; set; }
    public event Action OnCaptureEvent;
    public Action OnCapture { set { OnCaptureEvent += value; } }

    public void Capture()
    {
      if (State && (Condition is null || Condition.Invoke())) OnCaptureEvent?.Invoke();
    }
  }

  public class DebugGate<T1>
  {
    public bool State { get; set; }
    public Func<T1, bool> Condition { get; set; }
    public event Action<T1> OnCaptureEvent;
    public Action<T1> OnCapture { set { OnCaptureEvent += value; } }

    public void Capture(T1 arg1)
    {
      if (State && (Condition is null || Condition.Invoke(arg1))) OnCaptureEvent?.Invoke(arg1);
    }
  }

  public class DebugGate<T1, T2>
  {
    public bool State { get; set; }
    public Func<T1, T2, bool> Condition { get; set; }
    public event Action<T1, T2> OnCaptureEvent;
    public Action<T1, T2> OnCapture { set { OnCaptureEvent += value; } }
    public void Capture(T1 arg1, T2 arg2)
    {
      if (State && (Condition is null || Condition.Invoke(arg1, arg2))) OnCaptureEvent?.Invoke(arg1, arg2);
    }
  }




}