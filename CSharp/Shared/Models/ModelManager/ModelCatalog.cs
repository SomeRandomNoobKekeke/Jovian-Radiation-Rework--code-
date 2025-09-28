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
  public class ModelCatalog : IEnumerable
  {
    public Dictionary<Type, RadiationModel> ModelByType = new();
    public Dictionary<string, RadiationModel> ModelByName = new();
    public List<RadiationModel> ModelsInOrder = new();

    public static ModelCatalog FromModels(IEnumerable<RadiationModel> models)
    {
      ModelCatalog catalog = new ModelCatalog();
      foreach (RadiationModel model in models)
      {
        catalog.Add(model);
      }
      return catalog;
    }

    public void Add(RadiationModel model)
    {
      if (ModelByType.ContainsKey(model.GetType()))
        throw new ArgumentException($"{model.GetType()} already in the catalog");

      ModelByType[model.GetType()] = model;
      ModelByName[model.GetType().Name] = model;
      ModelsInOrder.Add(model);
    }

    IEnumerator IEnumerable.GetEnumerator() => ModelsInOrder.GetEnumerator();
  }



}