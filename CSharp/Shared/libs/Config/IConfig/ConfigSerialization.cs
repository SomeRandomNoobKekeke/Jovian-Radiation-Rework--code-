using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;

namespace BaroJunk
{
  public partial interface IConfig
  {
    public string ToText()
    {
      Dictionary<string, ConfigEntry> flat = GetAllFlat();
      StringBuilder sb = new StringBuilder();

      sb.Append("{\n");
      foreach (string key in flat.Keys)
      {
        sb.Append($"    {key}: [{ConfigLogger.WrapInColor(flat[key], "white")}],\n");
      }
      sb.Append("}");

      return sb.ToString();
    }


    public XElement ToXML()
    {
      XElement ToXMLRec(XElement element, IConfig config)
      {
        foreach (IConfigEntry entry in config.Entries)
        {
          if (entry.IsConfig)
          {
            IConfig subConfig = entry.Value as IConfig;
            if (subConfig is null) continue;
            element.Add(ToXMLRec(
              new XElement(entry.Name),
              subConfig
            ));
          }
        }

        foreach (IConfigEntry entry in config.Entries)
        {
          if (!entry.IsConfig)
          {
            element.Add(XMLParser.Serialize(entry));
          }
        }

        return element;
      }

      return ToXMLRec(new XElement(Name), this);
    }

    public void FromXML(XElement element)
    {
      foreach (XElement child in element.Elements())
      {
        IConfigEntry entry = Get(child.Name.ToString());
        if (!entry.IsValid) continue;

        if (entry.IsConfig)
        {
          IConfig subConfig = entry.Value as IConfig;

          if (subConfig is null)
          {
            subConfig = (IConfig)Activator.CreateInstance(entry.Type);
            entry.Value = subConfig;
          }

          subConfig.FromXML(child);
        }
        else
        {
          SimpleResult result = XMLParser.Parse(child, entry.Type);
          entry.Value = result.Result;
          if (!result.Ok) Logger.Warning(result.Details);
        }
      }
    }

    public Func<string[][]> ToHints()
    {
      return () => new string[][] { GetFlat().Keys.ToArray() };
    }
  }

}