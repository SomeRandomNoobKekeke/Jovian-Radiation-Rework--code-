using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

using Barotrauma;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;

namespace CrabUI
{
  public static class CUIPrefab
  {
    public static CUIHorizontalList TickboxWithLabel(float tickboxSize, string text)
    {
      CUIHorizontalList list = new CUIHorizontalList()
      {
        FitContent = new CUIBool2(true, true),
        BorderColor = Color.Transparent,
      };

      list["tickbox"] = new CUITickBox()
      {
        Absolute = new CUINullRect(w: tickboxSize, h: tickboxSize),
        Command = text,
      };

      list["text"] = new CUITextBlock()
      {
        Text = text,
        TextAlign = CUIAnchor.CenterLeft,
      };

      return list;
    }


  }
}