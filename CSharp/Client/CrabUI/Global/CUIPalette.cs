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

    public CUIColorPreset Component;
    public CUIColorPreset Frame;
    public CUIColorPreset Handle;
    public CUIColorPreset Wrapper;
    public CUIColorPreset Wrapper2;
    public CUIColorPreset Borderless;

    public CUIColorPreset Button;
    public CUIColorPreset ButtonOn;
    public CUIColorPreset ButtonHover;
    public CUIColorPreset ButtonOnHover;
    public CUIColorPreset ButtonDisabled;

    public CUIColorPreset Button2;
    public CUIColorPreset Button2On;
    public CUIColorPreset Button2Hover;
    public CUIColorPreset Button2OnHover;
    public CUIColorPreset Button2Disabled;

    public CUIColorPreset Button3;
    public CUIColorPreset Button3On;
    public CUIColorPreset Button3Hover;
    public CUIColorPreset Button3OnHover;
    public CUIColorPreset Button3Disabled;

    public CUIColorPreset DDButton;
    public CUIColorPreset DDButtonOn;
    public CUIColorPreset DDButtonHover;
    public CUIColorPreset DDButtonOnHover;
    public CUIColorPreset DDButtonDisabled;
    public CUIColorPreset DDOptionsBox;
    public CUIColorPreset DDOption;
    public CUIColorPreset DDOptionHover;

    public CUIColorPreset CloseButton;
    public CUIColorPreset CloseButtonOn;
    public CUIColorPreset CloseButtonHover;
    public CUIColorPreset CloseButtonOnHover;
    public CUIColorPreset CloseButtonDisabled;

    public CUIColorPreset Input;
    public CUIColorPreset InputHighlited;
    public CUIColorPreset InputValid;
    public CUIColorPreset InputInvalid;
    public CUIColorPreset InputCaret;
    public CUIColorPreset InputSelectionOverlay;
    public CUIColorPreset Text;
    public CUIColorPreset Text2;
    public CUIColorPreset Text3;

    public static CUIPalette Radiation = new CUIPalette() { Name = "Radiation", };


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
      Radiation = LoadFrom(Path.Combine(CUI.CUIPalettesPath, "Radiation.xml"));
      Radiation.SaveTo(Path.Combine(CUI.CUIPalettesPath, "Radiation.xml"));
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
            CUIExtensions.ParseColor(e.Attribute("Background").Value.ToString()),
            CUIExtensions.ParseColor(e.Attribute("Border").Value.ToString()),
            CUIExtensions.ParseColor(e.Attribute("Text").Value.ToString())
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

    public override string ToString() => $"CUIPalette {Name}";


  }
}