using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

using Barotrauma.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Voronoi2;


namespace JovianRadiationRework
{
  public interface IStepsCalculator
  {
    public float CalculateSteps(CampaignMode.TransitionType transitionType, float roundDuration);
  }
  public interface IRadiationMover
  {
    public void MoveRadiation(Radiation _, float steps);
  }

  public interface ILocationTransformer
  {
    public void TransformLocations(Radiation _);
  }

  public interface ICharacterDamager
  {
    public void DamageCharacter(Character character, float depthInRadiation, Radiation _);
  }

  public interface IRadiationUpdater
  {
    public void UpdateRadiation(Radiation __instance, float deltaTime);
  }

  public interface IRadAmountCalculator
  {
    public float CalculateAmount(Radiation __instance, Entity entity);
  }


}