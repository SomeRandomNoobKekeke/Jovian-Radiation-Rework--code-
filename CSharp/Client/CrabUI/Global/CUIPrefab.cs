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

namespace CrabUI_JovianRadiationRework
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

    public static CUIHorizontalList InputWithValidation(Type T, string command)
    {
      CUIHorizontalList list = new CUIHorizontalList()
      {
        FitContent = new CUIBool2(true, true),
        BorderColor = Color.Transparent,
      };

      list["label"] = new CUITextBlock()
      {
        Absolute = new CUINullRect(w: 50),
        Text = T.Name,
        TextAlign = CUIAnchor.Center,
        Style = new CUIStyle(){
          {"BackgroundColor", "CUIPalette.Current.Text3.Background"},
          {"BorderColor", "CUIPalette.Current.Text3.Border"},
        },
      };

      list["input"] = new CUITextInput()
      {
        FillEmptySpace = new CUIBool2(true, false),
        Command = command,
        Consumes = command,
      };

      return list;
    }


  }
}