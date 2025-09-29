using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public interface IFacade { }
  public interface IFacadeProvider
  {
    public IFacade Get(Type facadeType);
  }

  public partial class DefaultFacadesProvider : IFacadeProvider
  {
    public static DefaultFacadesProvider Instance = new();

    public Dictionary<Type, Func<IFacade>> FacadeFactories = new()
    {
      [typeof(ILevel)] = () => new DefaultCurrentLevelFacade(),
      [typeof(IRadiationAccessor)] = () => new DefaultRadiationAccessor(),
    };

    public IFacade Get(Type facadeType) => FacadeFactories.GetValueOrDefault(facadeType)?.Invoke();
  }
}