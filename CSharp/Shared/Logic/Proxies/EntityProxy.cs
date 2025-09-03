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
  public interface IEntity
  {
    public Vector2 WorldPosition { get; }
  }

  public class EntityProxy : IEntity
  {
    public Vector2 WorldPosition => entity.WorldPosition;

    private Entity entity;
    public EntityProxy(Entity entity) => this.entity = entity;
  }

}