using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

using Barotrauma.Abilities;
using Barotrauma.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml.Linq;

namespace JovianRadiationRework
{
  public interface IRadiation
  {
    public bool Enabled { get; }
    public float Amount { get; set; }
    public IRadiationParams Params { get; }
    public float DepthInRadiation(IEntity entity);
    public float DepthInRadiation(ILocation location);
  }

  public class RadiationProxy : IRadiation
  {
    public float Amount { get => radiation.Amount; set => radiation.Amount = value; }
    public bool Enabled => radiation.Enabled;

    public IRadiationParams Params { get; }

    public float DepthInRadiation(ILocation location)
    {
      return DepthInRadiation(location.MapPosition);
    }

    private float DepthInRadiation(Vector2 pos)
    {
      return Amount - pos.X;
    }
    public float DepthInRadiation(IEntity entity)
    {
      if (!Enabled) { return 0; }

      if (new LevelProxy(Level.Loaded) is { Type: LevelData.LevelType.LocationConnection, StartLocation: { } startLocation, EndLocation: { } endLocation } level)
      {
        // Approximate how far between the level start and end points the entity is on the map
        float distanceNormalized = MathHelper.Clamp((entity.WorldPosition.X - level.StartPosition.X) / (level.EndPosition.X - level.StartPosition.X), 0.0f, 1.0f);
        var (startX, startY) = startLocation.MapPosition;
        var (endX, endY) = endLocation.MapPosition;
        Vector2 mapPos = new Vector2(startX, startY) + (new Vector2(endX - startX, endY - startY) * distanceNormalized);

        return DepthInRadiation(mapPos);
      }

      return 0;
    }


    private Radiation radiation;
    public RadiationProxy(Radiation radiation)
    {
      this.radiation = radiation;
      Params = new RadiationParamsProxy(radiation.Params);
    }
  }

}