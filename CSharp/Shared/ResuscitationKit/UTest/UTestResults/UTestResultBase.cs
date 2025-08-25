using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace JovianRadiationRework
{
  public abstract class UTestResultBase
  {
    public virtual object Result { get; set; }

    // public virtual bool Equals(object obj);
  }
}