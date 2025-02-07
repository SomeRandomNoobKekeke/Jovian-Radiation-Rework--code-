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
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace CrabUI_JovianRadiationRework
{


  // I'm not an expert in color theory, but at least some order
  public record struct CUIColorPreset(Color Background, Color Border, Color Text);

  /// <summary>
  /// Contains some predefined values for components, mostly for colors  
  /// CUIPalette.Current is selected palette used by style resolver
  /// If you change it all components will recalculate their props
  /// There's just one palette for now :( , more to come
  /// </summary>
  public class CUIPalette
  {
    internal static void InitStatic()
    {
      CUI.OnInit += () =>
      {
        LoadPalettes();
      };
      CUI.OnDispose += () =>
      {

      };
    }

    //TODO what if object has to be null? mb return CUIResult?
    public static object Extract(string nestedName)
    {
      string[] names = nestedName.Split('.');
      if (names.Length == 0) return null;

      object result = null;

      FieldInfo fi = typeof(CUIPalette).GetField(names[0], AccessTools.all);
      PropertyInfo pi = typeof(CUIPalette).GetProperty(names[0], AccessTools.all);

      if (fi != null) result = fi.GetValue(null);
      if (pi != null) result = pi.GetValue(null);

      foreach (string name in names.Skip(1))
      {
        fi = result.GetType().GetField(name, AccessTools.all);
        pi = result.GetType().GetProperty(name, AccessTools.all);

        if (fi != null)
        {
          result = fi.GetValue(result);
          continue;
        }

        if (pi != null)
        {
          result = pi.GetValue(result);
          continue;
        }

        return null;
      }

      return result;
    }

    public string Name = "";

    public static CUIColorPreset Unset => new CUIColorPreset(Color.Orange, Color.Red, Color.Red);

    //TODO Ok, that's too much, unhardcode these
    public CUIColorPreset Component = Unset;
    public CUIColorPreset Frame = Unset;
    public CUIColorPreset Handle = Unset;
    public CUIColorPreset HandleGrabbed = Unset;
    public CUIColorPreset Wrapper = Unset;
    public CUIColorPreset Wrapper2 = Unset;
    public CUIColorPreset Borderless = Unset;

    public CUIColorPreset H1 = Unset;
    public CUIColorPreset H2 = Unset;
    public CUIColorPreset H3 = Unset;
    public CUIColorPreset H4 = Unset;

    public CUIColorPreset Button = Unset;
    public CUIColorPreset ButtonOn = Unset;
    public CUIColorPreset ButtonHover = Unset;
    public CUIColorPreset ButtonOnHover = Unset;
    public CUIColorPreset ButtonDisabled = Unset;

    public CUIColorPreset Button2 = Unset;
    public CUIColorPreset Button2On = Unset;
    public CUIColorPreset Button2Hover = Unset;
    public CUIColorPreset Button2OnHover = Unset;
    public CUIColorPreset Button2Disabled = Unset;

    public CUIColorPreset Button3 = Unset;
    public CUIColorPreset Button3On = Unset;
    public CUIColorPreset Button3Hover = Unset;
    public CUIColorPreset Button3OnHover = Unset;
    public CUIColorPreset Button3Disabled = Unset;

    public CUIColorPreset DDButton = Unset;
    public CUIColorPreset DDButtonOn = Unset;
    public CUIColorPreset DDButtonHover = Unset;
    public CUIColorPreset DDButtonOnHover = Unset;
    public CUIColorPreset DDButtonDisabled = Unset;
    public CUIColorPreset DDOptionsBox = Unset;
    public CUIColorPreset DDOption = Unset;
    public CUIColorPreset DDOptionHover = Unset;

    public CUIColorPreset CloseButton = Unset;
    public CUIColorPreset CloseButtonOn = Unset;
    public CUIColorPreset CloseButtonHover = Unset;
    public CUIColorPreset CloseButtonOnHover = Unset;
    public CUIColorPreset CloseButtonDisabled = Unset;

    public CUIColorPreset Input = Unset;
    public CUIColorPreset InputHighlited = Unset;
    public CUIColorPreset InputValid = Unset;
    public CUIColorPreset InputInvalid = Unset;
    public CUIColorPreset InputCaret = Unset;
    public CUIColorPreset InputSelectionOverlay = Unset;
    public CUIColorPreset Text = Unset;
    public CUIColorPreset Text2 = Unset;
    public CUIColorPreset Text3 = Unset;

    public static CUIPalette Radiation = new CUIPalette() { Name = "Radiation", };
    public static CUIPalette Invisible = new CUIPalette() { Name = "Invisible", };

    public static CUIPalette Default => Radiation;
    private static CUIPalette current = Default;
    /// <summary>
    /// Palette used by style resolver
    /// </summary>
    public static CUIPalette Current
    {
      get => current;
      set
      {
        current = value ?? Default;
        CUIGlobalStyleResolver.OnPaletteChange(current);
      }
    }

    static CUIPalette()
    {

    }

    public static void LoadPalettes()
    {
      //TODO add code for loading arbitrary palettes

      // I can load it like this because it's hardcoded default palette
      Stopwatch sw = new Stopwatch();
      sw.Restart();
      Radiation = LoadFrom(Path.Combine(CUI.CUIPalettesPath, "Radiation.xml"));
      //Radiation.SaveTo(Path.Combine(CUI.CUIPalettesPath, "Radiation.xml"));
      sw.Stop();
      //CUI.Info($"Default palette loaded in {sw.ElapsedMilliseconds}ms");
    }

    public static CUIPalette LoadFrom(string path)
    {
      CUIPalette palette = new CUIPalette();
      try
      {
        XDocument xdoc = XDocument.Load(path);
        XElement root = xdoc.Root;
        palette.Name = root.Attribute("Name").Value.ToString();

        foreach (XElement e in root.Elements())
        {
          FieldInfo fi = typeof(CUIPalette).GetField(e.Name.ToString());
          if (fi == null)
          {
            CUI.Warning($"Palette {palette.Name} contains a field {e.Name} that's not supported by C# class");
            continue;
          }

          CUIColorPreset preset = new CUIColorPreset(
            CUIExtensions.ParseColor(e.Attribute("Background")?.Value.ToString() ?? "0,0,0,0"),
            CUIExtensions.ParseColor(e.Attribute("Border")?.Value.ToString() ?? "0,0,0,0"),
            CUIExtensions.ParseColor(e.Attribute("Text")?.Value.ToString() ?? "0,0,0,0")
          );

          fi.SetValue(palette, preset);
        }
      }
      catch (Exception e)
      {
        CUI.Warning($"Failed to load palette from {path}");
        CUI.Warning(e);
      }

      return palette;
    }

    public void SaveTo(string path)
    {
      try
      {
        XDocument xdoc = new XDocument(new XElement("Palette"));
        XElement root = xdoc.Root;
        root.Add(new XAttribute("Name", Name));

        foreach (FieldInfo fi in typeof(CUIPalette).GetFields().Where(fi => fi.FieldType == typeof(CUIColorPreset)).OrderBy((fi) => fi.Name))
        {
          CUIColorPreset value = (CUIColorPreset)fi.GetValue(this);
          XElement preset = new XElement(fi.Name);
          preset.Add(new XAttribute("Background", CUIExtensions.ColorToString(value.Background)));
          preset.Add(new XAttribute("Border", CUIExtensions.ColorToString(value.Border)));
          preset.Add(new XAttribute("Text", CUIExtensions.ColorToString(value.Text)));
          root.Add(new XElement(preset));
        }

        xdoc.Save(path);
      }
      catch (Exception e)
      {
        CUI.Warning($"Failed to save palette to {path}");
        CUI.Warning(e);
      }
    }


    public static void PaletteDemo()
    {
      CUIFrame frame = new CUIFrame()
      {
        Absolute = new CUINullRect(0, 0, 500, 500),
        Anchor = CUIAnchor.Center,
      };

      frame.Layout = new CUILayoutVerticalList();

      frame["header"] = new CUIHorizontalList()
      {
        FitContent = new CUIBool2(false, true),
        Direction = CUIDirection.Reverse,
      };
      frame["header"]["close"] = new CUICloseButton()
      {
        Absolute = new CUINullRect(0, 0, 20, 20),
        AddOnMouseDown = (e) => frame.RemoveSelf(),
      };
      frame["content"] = new CUIVerticalList()
      {
        FillEmptySpace = new CUIBool2(false, true),
        Scrollable = true,
      };

      void AddComponent(Type T)
      {
        CUIComponent wrapper = new CUIHorizontalList()
        {
          Absolute = new CUINullRect(h: 20),
          CullChildren = false,
        };

        wrapper.Append(new CUITextBlock(T.Name)
        {
          Absolute = new CUINullRect(w: 130),
        });

        CUIComponent target = (CUIComponent)Activator.CreateInstance(T);
        target.FillEmptySpace = new CUIBool2(true, false);
        wrapper.Append(target);

        frame["content"].Append(wrapper);
      }

      AddComponent(typeof(CUIComponent));
      AddComponent(typeof(CUIButton));
      AddComponent(typeof(CUIToggleButton));
      AddComponent(typeof(CUIMultiButton));
      AddComponent(typeof(CUIDropDown));
      AddComponent(typeof(CUITextBlock));
      AddComponent(typeof(CUITextInput));
      AddComponent(typeof(CUIHorizontalList));
      AddComponent(typeof(CUIVerticalList));


      CUI.TopMain.Append(frame);
    }

    public override string ToString() => $"CUIPalette {Name}";


  }
}