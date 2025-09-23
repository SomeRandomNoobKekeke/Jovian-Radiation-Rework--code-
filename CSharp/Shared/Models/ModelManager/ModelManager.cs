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
    public LayerCatalog Layers = new()
    {
      new VanillaRadiationModel(),
    };

    public static RadiationModel Base => new VanillaRadiationModel();
    public RadiationModel Current { get; set; } = Base;


    public void Recombine()
    {
      Current = Base;

      foreach (ModelLayer layer in Layers)
      {
        if (layer.Enabled) Current.Combine(layer.Model);
      }
    }
  }



}