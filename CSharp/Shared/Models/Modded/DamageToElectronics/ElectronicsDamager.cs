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
  public partial class DamageToElectronicsModel
  {
    public class ModdedElectronicsDamager : ICharacterDamager
    {
      public ModelSettings Settings { get; set; }

      public double updateTimer;

      public void DamageElectronics(Radiation _, float deltaTime)
      {
        if (updateTimer > 0)
        {
          updateTimer -= deltaTime;
          return false;
        }

        updateTimer = Settings.DamageInterval;
      }



      public bool Debug = false;

      public List<Item> Damagable = new List<Item>();

      public bool IsValid(Item i)
      {
        bool isRepairable = i.GetComponent<Repairable>() != null;
        bool isPowerTransfer = i.GetComponent<PowerTransfer>() != null;
        bool isPowerContainer = i.GetComponent<PowerContainer>() != null;
        bool isSteering = i.GetComponent<Steering>() != null;
        bool isSonar = i.GetComponent<Sonar>() != null;

        bool isFromMainSub = i.Submarine?.Info.Type == SubmarineType.Player;

        return isFromMainSub && isRepairable && (isPowerTransfer || isPowerContainer || isSteering || isSonar);
      }

      public void FindItems()
      {
        foreach (Item i in Item.ItemList)
        {
          if (IsValid(i)) Damagable.Add(i);
        }
      }

      public void DamageItems()
      {
        if (Submarine.MainSub == null) return;

        float damage = Math.Max(0, EntityRadiationAmount(Submarine.MainSub)) * settings.Mod.ElectronicsDamageMultiplier;
        damage = Math.Clamp(damage, 0, settings.Mod.MaxDamageToElectronics);

        if (Debug) Info($"Damaging Electronics {damage}");


        foreach (Item i in Damagable)
        {
          float last = i.Condition;
          i.Condition -= damage;
          if (Debug) Mod.Log($"{i}.Condition {last} -> {i.Condition}");
        }
      }

    }
  }
}