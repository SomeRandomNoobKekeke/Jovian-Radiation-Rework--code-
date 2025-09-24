using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

namespace JovianRadiationRework
{
  public class ModelLayer
  {
    public RadiationModel Model { get; set; }
    public bool Enabled { get; set; } = true;//HACK temporary

    public ModelLayer(RadiationModel model) => Model = model;
  }



}