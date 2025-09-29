using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public class FakeRadiationAccessorData
  {
    public bool Enabled { get; set; }
    public float Amount { get; set; }
  }

  public class FakeRadiationAccessor : IRadiationAccessor
  {
    public FakeRadiationAccessorData Data { get; set; }
    public bool Enabled(Radiation radiation) => Data.Enabled;
    public float Amount(Radiation radiation) => Data.Amount;
  }



}