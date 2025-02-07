using System;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using CrabUI_JovianRadiationRework;

namespace JovianRadiationRework
{
  public class CUIAlert : CUIFrame
  {
    public static void Open(string text = "")
    {
      CUIAlert dialog = new CUIAlert(text);
      CUI.TopMain.Append(dialog);
    }

    public CUIAlert(string text = "")
    {
      Layout = new CUILayoutVerticalList();
      Anchor = CUIAnchor.Center;
      Absolute = new CUINullRect(w: 200);
      FitContent = new CUIBool2(false, true);
      Resizible = false;
      this["message"] = new CUITextBlock(text)
      {
        TextScale = 1.2f,
        TextAlign = CUIAnchor.Center,
        Style = new CUIStyle(){
          {"BackgroundColor", "CUIPalette.Current.Text2.Background"},
          {"BorderColor", "CUIPalette.Current.Component.Border"},
          {"TextColor", "CUIPalette.Current.Text2.Text"},
        },
      };

      this["ok"] = new CUIButton("OK")
      {
        AddOnMouseDown = (e) => this.RemoveSelf(),
      };
    }
  }
}