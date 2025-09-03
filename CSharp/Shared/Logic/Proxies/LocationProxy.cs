using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{


  public interface ILocation
  {
    public Vector2 MapPosition { get; }
  }

  public class LocationProxy : ILocation
  {
    public Vector2 MapPosition => location.MapPosition;

    private Location location;
    public LocationProxy(Location location) => this.location = location;
  }

}