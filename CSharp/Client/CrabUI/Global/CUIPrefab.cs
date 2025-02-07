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

    //TODO this is now too specific and shouldn't be here
    public static CUIHorizontalList InputWithValidation(PropertyInfo pi, string command)
    {
      string ToUserFriendly(Type T)
      {
        if (T == typeof(bool)) return "Boolean";
        if (T == typeof(int)) return "Integer";
        if (T == typeof(float)) return "Float";
        return T.Name;
      }

      CUIHorizontalList list = new CUIHorizontalList()
      {
        FitContent = new CUIBool2(true, true),
        BorderColor = Color.Transparent,
      };

      list["input"] = new CUITextInput()
      {
        AbsoluteMin = new CUINullRect(w: 100),
        Relative = new CUINullRect(w: 0.3f),
        Command = command,
        Consumes = command,
        VatidationType = pi.PropertyType,
      };

      list["label"] = new CUITextBlock()
      {
        FillEmptySpace = new CUIBool2(true, false),
        Text = $"{ToUserFriendly(pi.PropertyType)} {pi.Name}",
        TextAlign = CUIAnchor.CenterLeft,
        BackgroundSprite = new CUISprite("gradient.png"),

        Style = new CUIStyle(){
          {"BackgroundColor", "CUIPalette.Current.Text3.Background"},
          {"BorderColor", "CUIPalette.Current.Text3.Border"},
          {"TextColor", "CUIPalette.Current.Text3.Text"},
        },
      };

      return list;
    }


  }
}