using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.IO;

using Barotrauma;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using System.Xml;
using System.Xml.Linq;

namespace CrabUI_JovianRadiationRework
{

  public class CommandAttribute : System.Attribute { }

  /// <summary>
  /// Can be dispatched up the component tree to notify parent about something 
  /// add pass some event data without creating a hard link
  /// </summary>
  /// <param name="Name"></param>
  public record CUICommand(string Name, object data = null);

  /// <summary>
  /// Can be dispatched down the component tree to pass some data to the children
  /// without creating a hard link
  /// </summary>
  public record CUIData(string Name, object data = null);
  public partial class CUIComponent
  {
    /// <summary>
    /// All commands
    /// </summary>
    public Dictionary<string, Action<object>> Commands { get; set; } = new();

    /// <summary>
    /// Manually adds command
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddCommand(string name, Action<object> action) => Commands.Add(name, action);
    public void RemoveCommand(string name) => Commands.Remove(name);

    /// <summary>
    /// Methods ending in "Command" will be added as commands
    /// </summary>
    public virtual void AddCommands()
    {
      foreach (MethodInfo mi in this.GetType().GetMethods())
      {
        if (Attribute.IsDefined(mi, typeof(CommandAttribute)))
        {
          try
          {
            string name = mi.Name;
            if (name != "Command" && name.EndsWith("Command"))
            {
              name = name.Substring(0, name.Length - "Command".Length);
            }
            AddCommand(name, mi.CreateDelegate<Action<object>>(this));
          }
          catch (Exception e)
          {
            Info($"{e.Message}\nMethod: {this.GetType()}.{mi.Name}");
          }
        }
      }
    }

    /// <summary>
    /// Dispathes command up the component tree until someone consumes it
    /// </summary>
    /// <param name="command"></param>
    public void DispatchUp(CUICommand command)
    {
      if (Commands.ContainsKey(command.Name)) Execute(command);
      else Parent?.DispatchUp(command);
    }

    /// <summary>
    /// Dispathes command down the component tree until someone consumes it
    /// </summary>
    public void DispatchDown(CUIData data)
    {
      foreach (CUIComponent child in Children)
      {
        if (child.Consumes == data.Name)
        {
          child.Consume(data.data);
        }
        else
        {
          child.DispatchDown(data);
        }
      }
    }

    /// <summary>
    /// Will execute action corresponding to this command
    /// </summary>
    /// <param name="commandName"></param>
    public void Execute(CUICommand command) => Commands.GetValueOrDefault(command.Name)?.Invoke(command.data);

    public virtual void Consume(object o) { }
  }
}