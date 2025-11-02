using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{


  public interface ILevel : IFacade
  {
    public bool IsLoaded { get; }
    public LevelData.LevelType Type { get; }

    public Vector2 StartPosition { get; }
    public Vector2 EndPosition { get; }
    public Vector2 StartLocation_MapPosition { get; }
    public Vector2 EndLocation_MapPosition { get; }
  }

  public class DefaultCurrentLevelFacade : ILevel
  {
    public bool IsLoaded => Level.Loaded is not null;
    public LevelData.LevelType Type => Level.Loaded.Type;

    public Vector2 StartPosition => Level.Loaded.StartPosition;
    public Vector2 EndPosition => Level.Loaded.EndPosition;
    public Vector2 StartLocation_MapPosition => Level.Loaded.StartLocation.MapPosition;
    public Vector2 EndLocation_MapPosition => Level.Loaded.EndLocation.MapPosition;

    public override string ToString() => Logger.Wrap.IDictionary(new Dictionary<string, object>()
    {
      ["IsLoaded"] = IsLoaded,
      ["Type"] = Type,
      ["StartPosition"] = StartPosition,
      ["EndPosition"] = EndPosition,
      ["StartLocation_MapPosition"] = StartLocation_MapPosition,
      ["EndLocation_MapPosition"] = EndLocation_MapPosition,
    });
  }



}