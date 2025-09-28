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
    //TODO put it somewhere else
    //Also lol, i didn't know XmlWriter can write to stringbuilder
    public static string Beautify(XDocument doc)
    {
      StringBuilder sb = new StringBuilder();
      XmlWriterSettings settings = new XmlWriterSettings
      {
        Indent = true,
        IndentChars = "    ",
        NewLineChars = "\r\n",
        NewLineHandling = NewLineHandling.Replace,
        OmitXmlDeclaration = true
      };
      using (XmlWriter writer = XmlWriter.Create(sb, settings))
      {
        doc.Save(writer);
      }
      return sb.ToString();
    }

    public string ToText()
    {
      if (Settings.PrintAsXML)
      {
        XDocument xdoc = new XDocument();
        xdoc.Add(ToXML());
        return Beautify(xdoc);
      }

      StringBuilder sb = new StringBuilder();

      void ToTextRec(string offset, IConfig config)
      {
        foreach (IConfigEntry entry in config.Entries)
        {
          if (entry.IsConfig)
          {
            IConfig subConfig = entry.Value as IConfig;
            if (subConfig is null) continue;
            sb.Append($"{offset}{entry.Name}:\n");
            ToTextRec($"{offset}       |", subConfig);
            sb.Append($"{offset}        \n");
          }
        }

        foreach (IConfigEntry entry in config.Entries)
        {
          if (!entry.IsConfig)
          {
            sb.Append($"{offset}{entry.Name}: {ConfigLogger.WrapInColor(entry.Value, "white")}\n");
          }
        }
      }

      ToTextRec("", this);
      sb.Remove(sb.Length - 1, 1);
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