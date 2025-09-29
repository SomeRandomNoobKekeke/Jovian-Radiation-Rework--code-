using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public interface IRadiationAccessor : IFacade
  {
    public bool Enabled(Radiation radiation);
    public float Amount(Radiation radiation);
  }

  public class DefaultRadiationAccessor : IRadiationAccessor
  {
    public bool Enabled(Radiation radiation) => radiation.Enabled;
    public float Amount(Radiation radiation) => radiation.Amount;
  }



}