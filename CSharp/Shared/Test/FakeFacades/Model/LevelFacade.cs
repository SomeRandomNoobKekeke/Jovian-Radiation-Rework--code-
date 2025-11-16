using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public class FakeCurrentLevelData
  {
    public bool IsLoaded { get; set; }
    public LevelData.LevelType Type { get; set; }

    public Vector2 StartPosition { get; set; }
    public Vector2 EndPosition { get; set; }
    public Vector2 StartLocation_MapPosition { get; set; }
    public Vector2 EndLocation_MapPosition { get; set; }
  }

  public class FakeCurrentLevelFacade : ILevel
  {
    public FakeCurrentLevelData Data { get; set; }


    public bool IsLoaded => Data.IsLoaded;
    public LevelData.LevelType Type => Data.Type;

    public bool IsEndBiome { get; set; } = false;
    public Vector2 StartPosition => Data.StartPosition;
    public Vector2 EndPosition => Data.EndPosition;
    public Vector2 StartLocation_MapPosition => Data.StartLocation_MapPosition;
    public Vector2 EndLocation_MapPosition => Data.EndLocation_MapPosition;
  }



}