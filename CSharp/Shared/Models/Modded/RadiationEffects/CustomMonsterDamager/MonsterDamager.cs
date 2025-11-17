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
  public partial class CustomMonsterDamagerModel
  {
    /// <summary>
    /// It's full vanilla, but you can disable monster buffing
    /// </summary>
    public class CustomMonsterDamager : IMonsterDamager
    {
      public AfflictionPrefab JovesRage;
      public Affliction JovesRageBuff;

      public ModelSettings Settings { get; set; }
      public CustomMonsterDamagerModel Model { get; set; }

      public void DamageMonster(Character character, float radAmount, Radiation _)
      {
        if (!Settings.BuffMonsters) return;

        if (radAmount > 0)
        {
          Model.DebugLog($"Buffing [{character}] with [{JovesRageBuff}]");
          character.CharacterHealth.ApplyAffliction(
            character.AnimController?.MainLimb,
            JovesRageBuff
          );
        }
      }

      public CustomMonsterDamager()
      {
        JovesRage = AfflictionPrefab.Prefabs["jovesrage"];
        JovesRageBuff = JovesRage.Instantiate(25);
      }
    }
  }
}