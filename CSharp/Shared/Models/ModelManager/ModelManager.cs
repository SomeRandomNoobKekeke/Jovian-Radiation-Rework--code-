using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;


using Barotrauma;
using HarmonyLib;
using System.IO;


namespace JovianRadiationRework
{
  public class ModelManager
  {
    public ModelCatalog Models = new ModelCatalog();


    public static RadiationModel Base => new VanillaRadiationModel();
    public RadiationModel Current { get; set; } = Base;

    public RadiationModel ModelByType(Type T) => Models.ModelByType.GetValueOrDefault(T);

    public void SetModelState(Type T, bool state)
    {
      RadiationModel model = Models.ModelByType.GetValueOrDefault(T);
      if (model is null) throw new ArgumentException($"Can't set state of a missing model [{T.Name}]");
      model.Enabled = state;
      Recombine();
    }

    public void SetModelState(string name, bool state)
    {
      RadiationModel model = Models.ModelByName.GetValueOrDefault(name);
      if (model is null) throw new ArgumentException($"Can't set state of a missing model [{name}]");
      model.Enabled = state;
      Recombine();
    }

    public void SyncModelStates(EnabledModels enabled)
    {
      foreach (ConfigEntry entry in enabled.GetEntries())
      {
        RadiationModel model = Models.ModelByName.GetValueOrDefault(entry.Key);
        if (model is null) throw new ArgumentException($"Can't sync state of a missing model [{entry.Key}]");
        model.Enabled = (bool)entry.Value;
      }
      Recombine();
    }


    public void Recombine()
    {
      Current = Base;

      foreach (RadiationModel model in Models)
      {
        if (model.Enabled) Current.Combine(model);
      }
    }

    public void ScanModels()
    {
      List<Type> modelTypes = new();

      foreach (Type T in Assembly.GetCallingAssembly().GetTypes())
      {
        if (T.IsAssignableTo(typeof(RadiationModel)))
        {
          modelTypes.Add(T);
        }
      }

      modelTypes.Remove(typeof(RadiationModel));

      List<RadiationModel> models = new List<RadiationModel>();
      foreach (Type T in modelTypes)
      {
        try
        {
          models.Add(Activator.CreateInstance(T) as RadiationModel);
        }
        catch (Exception e) { Mod.Logger.Error(e); }
      }

      Models = ModelCatalog.FromModels(models);
      Recombine();
      // DumpModels();
    }

    /// <summary>
    /// This goes into MainConfig.EnabledModels
    /// </summary>
    /// <param name="path"></param>
    public void DumpModels(string path = "Ignore/models.txt")
    {
      using (StreamWriter outputFile = new StreamWriter(Path.Combine(ModInfo.ModDir<Mod>(), path)))
      {
        foreach (RadiationModel model in Models)
        {
          outputFile.WriteLine(
            $"public bool {model.GetType().Name} {{get; set; }} = true;"
          );
        }
      }
    }

    public void PopulateWith(IEnumerable<RadiationModel> models)
    {
      Models = ModelCatalog.FromModels(models);
      Recombine();
    }

    public ModelManager() { }
    public ModelManager(IEnumerable<RadiationModel> models)
    {
      PopulateWith(models);
    }
  }



}