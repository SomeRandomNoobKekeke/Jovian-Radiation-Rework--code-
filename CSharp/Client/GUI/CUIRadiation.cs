using System;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CrabUI_JovianRadiationRework
{
  public class CUIRadiation : CUIWater
  {
    public class Splash
    {
      public int X;
      public int Y;
      public int TicksToLive;
      public double Frequency;
      public Splash(int x, int y)
      {
        (X, Y) = (x, y);

        Frequency = CUI.Random.NextDouble() * 0.2 + 0.01;
        TicksToLive = (int)Math.Clamp(10 / Frequency, 10, 300);
      }
    }

    public List<Splash> Splashes = new List<Splash>();

    public override void UpdateSelf()
    {

      if (Splashes.Count < 30)
      {
        int w = Pool1.GetUpperBound(0);
        int h = Pool1.GetUpperBound(1);

        Splashes.Add(new Splash(2, CUI.Random.Next(2, h - 1)));
        //Splashes.Add(new Splash(w - 1, CUI.Random.Next(2, h - 1)));
      }

      foreach (Splash s in Splashes)
      {
        Pool1[s.X, s.Y] = (float)Math.Sin(s.TicksToLive * s.Frequency) * DropSize;
        s.TicksToLive--;
      }

      Splashes = Splashes.Where(s => s.TicksToLive >= 0).ToList();
    }

    public CUIRadiation(int x, int y) : base(x, y)
    {
      DropSize = 4.0f;
      Omega = 1.999f;

      UpdateInterval = 1.0 / 60.0;

      ColorPalette = new Color[]{
        new Color(0,0,0,0),
        new Color(0,0,255),
        new Color(0,100,255),
      };


    }

  }
}