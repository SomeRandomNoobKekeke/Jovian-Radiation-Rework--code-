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
    public void EnableModel(Type T)
    {
      RadiationModel model = Models.ModelByType.GetValueOrDefault(T);
      if (model is null) throw new ArgumentException($"Tried to enable missing model [{T.Name}]");
      model.Enabled = true;
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
      DumpModels();
    }

    public void DumpModels(string path = "Ignore/models.txt")
    {
      using (StreamWriter outputFile = new StreamWriter(Path.Combine(ModInfo.ModDir<Mod>(), path)))
      {
        foreach (RadiationModel model in Models)
        {
          outputFile.WriteLine(model.GetType().Name);
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