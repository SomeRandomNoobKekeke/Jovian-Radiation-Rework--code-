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
  /// <summary>
  /// xd
  /// </summary>
  public interface IStaticLevel
  {
    public ILevel Loaded { get; }
  }

  public class StaticLevelProxy : IStaticLevel
  {
    public ILevel Loaded => new LevelProxy(Level.Loaded);
  }


  public interface ILevel
  {
    public LevelData.LevelType Type { get; }
    public ILocation StartLocation { get; }
    public ILocation EndLocation { get; }

    public Vector2 StartPosition { get; }
    public Vector2 EndPosition { get; }
  }

  public class LevelProxy : ILevel
  {
    public LevelData.LevelType Type => level.Type;
    public ILocation StartLocation { get; }
    public ILocation EndLocation { get; }
    public Vector2 StartPosition => level.StartPosition;
    public Vector2 EndPosition => level.EndPosition;

    private Level level;
    public LevelProxy(Level level)
    {
      this.level = level;
      StartLocation = new LocationProxy(level.StartLocation);
      EndLocation = new LocationProxy(level.EndLocation);
    }
  }

}