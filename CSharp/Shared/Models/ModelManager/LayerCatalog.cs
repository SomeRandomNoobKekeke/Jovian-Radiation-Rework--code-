using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

namespace JovianRadiationRework
{
  public class LayerCatalog : IEnumerable
  {
    public Dictionary<Type, ModelLayer> LayerByType = new();
    public List<ModelLayer> LayersInOrder = new();

    public void Add(RadiationModel model)
    {
      if (LayerByType.ContainsKey(model.GetType()))
        throw new ArgumentException($"{model.GetType()} already in the catalog");

      LayerByType[model.GetType()] = new ModelLayer(model);
      LayersInOrder.Add(LayerByType[model.GetType()]);
    }

    IEnumerator IEnumerable.GetEnumerator() => LayersInOrder.GetEnumerator();
  }



}