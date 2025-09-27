using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using System.Text;



namespace JovianRadiationRework
{

  /// <summary>
  /// It's just a collection of aspect implementations
  /// It should populate itself
  /// </summary>
  public partial class RadiationModel
  {
    public IEnumerable<PropertyInfo> AspectProps =>
      this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
      .Where(pi => pi.PropertyType.IsAssignableTo(typeof(IModelAspect)));

    public IEnumerable<IModelAspect> Aspects
      => AspectProps.Select(pi => pi.GetValue(this) as IModelAspect)
        .Where(aspect => aspect is not null);

    public IEnumerable<string> AspectNames => AspectProps.Select(pi => pi.Name);
    public bool IsComplete => AspectProps.All(pi => pi.GetValue(this) is not null);

    public Dictionary<Type, Type> AspectImplementations
      => this.GetType().GetNestedTypes()
        .Where(T => T.IsAssignableTo(typeof(IModelAspect)))
        .ToDictionary(
          T => T.GetInterfaces().First(i => i.IsAssignableTo(typeof(IModelAspect))),
          T => T
        );
    public Type SettingsType => this.GetType().GetNestedTypes().FirstOrDefault(T => T.IsAssignableTo(typeof(IConfig)));

    public IModelAspect GetAspect(string name)
       => this.GetType()
          .GetProperty(name, BindingFlags.Public | BindingFlags.Instance)
          .GetValue(this) as IModelAspect;

    public void SetAspect(string name, IModelAspect value)
      => this.GetType()
         .GetProperty(name, BindingFlags.Public | BindingFlags.Instance)
         .SetValue(this, value);


    //TODO optimize
    public void Combine(RadiationModel other)
    {
      foreach (string name in AspectNames)
      {
        if (other.GetAspect(name) is not null)
        {
          this.SetAspect(name, other.GetAspect(name));
        }
      }
    }



    /// <summary>
    /// It's sneaky and dishonest to the consumer, but it lets me minimize boilerplate code 
    /// and i think it's ok as long as i'm not getting confused 
    /// 
    /// All IModelAspect props in the model are initialized with new instances of aspect implementation classes from withing the model
    /// Then instace of Model settings is grabbed from Mod.Config and passed to aspect implementations
    /// </summary>
    private void Initialize()
    {
      Dictionary<Type, Type> ImplementationClasses = AspectImplementations;

      // ---------- Achtung! link to a static singleton ----------
      IConfig settings = Mod.Config.GetSubSettings(SettingsType);
      // ---------- Achtung! link to a static singleton ----------

      foreach (PropertyInfo pi in AspectProps)
      {
        IModelAspect implementation = pi.GetValue(this) as IModelAspect;

        if (pi.GetValue(this) is null)
        {
          Type implementationType = ImplementationClasses.GetValueOrDefault(pi.PropertyType);
          if (implementationType is null) continue;

          implementation = Activator.CreateInstance(
            implementationType
          ) as IModelAspect;

          pi.SetValue(this, implementation);
        }

        implementation.AcceptSettings(settings);
      }
    }

    public RadiationModel()
    {
      Initialize();
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();

      bool isComplete = true;

      sb.Append("{\n");
      foreach (string name in AspectNames)
      {
        IModelAspect aspect = GetAspect(name);
        if (aspect is null) isComplete = false;
        sb.Append($"    {name}: [{Logger.WrapInColor(aspect?.GetType().Name, "white")}],\n");
      }
      sb.Append("} ");
      sb.Append($"[{(isComplete ? "Complete" : "Incomplete")}]");

      return sb.ToString();
    }

  }



}