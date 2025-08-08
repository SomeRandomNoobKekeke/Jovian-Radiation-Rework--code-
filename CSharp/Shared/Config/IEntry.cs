using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;

namespace JovianRadiationRework
{
  public interface IEntry
  {
    public ConfigEntry Get(string propName)
      => new ConfigEntry();
  }
}