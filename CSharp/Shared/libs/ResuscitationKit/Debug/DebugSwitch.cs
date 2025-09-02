using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
namespace JovianRadiationRework
{

  public interface IDebugSwitch
  {
    public bool State { get; set; }
    public void On();
    public void Off();
    public void Toggle();

  }
  public class DebugSwitch : IDebugSwitch
  {
    public bool State { get; set; }
    public void On() => State = true;
    public void Off() => State = false;
    public void Toggle() => State = !State;

    public DebugSwitch() { }
    public DebugSwitch(bool state) => State = state;
  }


}