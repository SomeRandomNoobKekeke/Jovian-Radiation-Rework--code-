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
  public class CUISaveDialog : CUIFrame
  {
    public static void Open()
    {
      CUISaveDialog dialog = new CUISaveDialog();
      CUI.TopMain.Append(dialog);
    }

    public CUISaveDialog()
    {
      Layout = new CUILayoutVerticalList();
      Anchor = CUIAnchor.Center;
      FitContent = new CUIBool2(true, true);
      this["header"] = new CUITextBlock("Save current settings as:")
      {
        Padding = new Vector2(4, 4),
        FitContent = new CUIBool2(false, true),
        TextScale = 1.2f,
        TextAlign = CUIAnchor.Center,
        Style = new CUIStyle(){
          {"BackgroundColor", "CUIPalette.Current.Text2.Background"},
          {"BorderColor", "CUIPalette.Current.Component.Border"},
          {"TextColor", "CUIPalette.Current.Text2.Text"},
        },
      };
      this["input"] = new CUITextInput()
      {
        TextScale = 2.0f,
      };
      this["nav"] = new CUIHorizontalList()
      {
        FitContent = new CUIBool2(false, true),
      };

      this["nav"]["save"] = new CUIButton("save")
      {
        FillEmptySpace = new CUIBool2(true, false),
        TextScale = 1.2f,
        AddOnMouseDown = (e) =>
        {
          Mod.Instance?.Rad_Save_Command(new string[] { this.Get<CUITextInput>("input").Text });
          this.RemoveSelf();
        },
      };
      this["nav"]["cancel"] = new CUIButton("cancel")
      {
        FillEmptySpace = new CUIBool2(true, false),
        TextScale = 1.2f,
        AddOnMouseDown = (e) => this.RemoveSelf(),
      };

    }
  }
}