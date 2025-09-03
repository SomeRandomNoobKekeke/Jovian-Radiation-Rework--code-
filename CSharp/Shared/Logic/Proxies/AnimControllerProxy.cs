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
  public interface IAnimController
  {
    public ILimb MainLimb { get; }
  }

  public class AnimControllerProxy : IAnimController
  {
    public ILimb MainLimb { get; set; }


    private AnimController animController;
    public AnimControllerProxy(AnimController animController)
    {
      this.animController = animController;
      MainLimb = new LimbProxy(animController.MainLimb);
    }
  }

}