using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;


using Barotrauma;
using HarmonyLib;

namespace JovianRadiationRework
{
  public class ModelManager
  {
    public LayerCatalog Layers = new LayerCatalog();


    public static RadiationModel Base => new VanillaRadiationModel();
    public RadiationModel Current { get; set; } = Base;

    public RadiationModel ModelByType(Type T)
      => Layers.LayerByType.GetValueOrDefault(T)?.Model;
    public void EnableModel(Type T)
    {
      ModelLayer layer = Layers.LayerByType.GetValueOrDefault(T);
      if (layer is null) throw new ArgumentException($"Tried to enable missing model [{T.Name}]");
      layer.Enabled = true;
      Recombine();
    }


    public void Recombine()
    {
      Current = Base;

      foreach (ModelLayer layer in Layers)
      {
        if (layer.Enabled) Current.Combine(layer.Model);
      }
    }

    public void ScanModels()
    {
      List<Type> modelTypes = new();

      foreach (Type T in Assembly.GetCallingAssembly().GetTypes())
      {
        if (T.IsAssignableTo(typeof(RadiationModel)))
        {
          modelTypes.Add(T);
        }
      }

      modelTypes.Remove(typeof(RadiationModel));

      Layers = LayerCatalog.FromModels(
        modelTypes.Select(T => Activator.CreateInstance(T) as RadiationModel)
      );
      Recombine();
    }

    public void PopulateWith(IEnumerable<RadiationModel> models)
    {
      Layers = LayerCatalog.FromModels(models);
      Recombine();
    }

    public ModelManager() { }
    public ModelManager(IEnumerable<RadiationModel> models)
    {
      PopulateWith(models);
    }
  }



}