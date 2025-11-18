
using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using Barotrauma.Networking;
using System.Runtime.CompilerServices;

namespace BaroJunk
{
  public class IsolatedCommands
  {
    public Dictionary<string, Action<string[]>> Commands { get; set; } = new();

    public void Add(string name, Action<string[]> command) => Commands.Add(name, command);

    //TODO test and make it safe
    public void Execute(string command)
    {
      if (string.IsNullOrEmpty(command)) return;

      IEnumerable<string> parts = command.Split(' ').Where(part => part != "");

      Logger.Default.Log(Logger.Wrap.IEnumerable(parts));

      if (Commands.ContainsKey(parts.First()))
      {
        Commands[parts.First()].Invoke(parts.Skip(1).ToArray());
      }
    }
  }
}