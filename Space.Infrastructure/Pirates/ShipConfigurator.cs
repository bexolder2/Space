using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Space.Helpers.Interfaces;
using Space.Infrastructure.Deserializer;
using Space.Model.Constants;
using Space.Model.Enums;
using Space.Model.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Space.Infrastructure.Pirates
{
    public class ShipConfigurator
    {
        private List<Module> baseModules;
        private Dictionary<IBindableModel, Module> modules;

        public ShipConfigurator()
        {
            baseModules = new List<Module>();
            modules = new Dictionary<IBindableModel, Module>();
            InitializeModulesProperties();
            InitializeBaseModules();
        }

        public Pirate CreatePirate()
        {
            Pirate pirate = new Pirate();
            pirate.Spaceship = new Spaceship();
            pirate.Spaceship.ShipModules = new List<KeyValuePair<IBindableModel, Module>>();
            pirate.ShipLevel = GenerateShipLevel();

            foreach (var module in baseModules)
            {
                pirate.Spaceship.ShipModules
                    .Add(new KeyValuePair<IBindableModel, Module>(GetModule(pirate.ShipLevel, module), module));
            }

            int numberOfAdditionalGuns = GenerateNumberOfAdditionalGuns();
            int startIndex = baseModules.IndexOf(Module.Body) - 1;

            if (numberOfAdditionalGuns == 1)
            {
                pirate.Spaceship.ShipModules.RemoveAt(2);
                pirate.Spaceship.ShipModules.Insert(2, pirate.Spaceship.ShipModules.FirstOrDefault(x => x.Value == Module.Gun));

                if (numberOfAdditionalGuns == 2)
                {
                    pirate.Spaceship.ShipModules.Insert(12, pirate.Spaceship.ShipModules[1]);
                    pirate.Spaceship.ShipModules.Insert(13, pirate.Spaceship.ShipModules.FirstOrDefault(x => x.Value == Module.Gun));
                    pirate.Spaceship.ShipModules.Insert(14, pirate.Spaceship.ShipModules[1]);
                    pirate.Spaceship.ShipModules.Insert(15, pirate.Spaceship.ShipModules[1]);
                    pirate.Spaceship.ShipModules.Add(pirate.Spaceship.ShipModules.LastOrDefault());

                    if (numberOfAdditionalGuns == 3)
                    {
                        pirate.Spaceship.ShipModules.RemoveAt(15);
                        pirate.Spaceship.ShipModules.Insert(15, pirate.Spaceship.ShipModules.FirstOrDefault(x => x.Value == Module.Gun));
                    }
                }
            }

            return pirate;
        }

        private IBindableModel GetModule(Level level, Module type)
        {
            IBindableModel result = null;
            result = modules.FirstOrDefault(x => x.Value == type && ((BaseModel)x.Key).Level == level).Key;
            return result;
        } 

        private void InitializeModulesProperties()
        {
            var result = JsonDeserializer.InitializeModules();
            FillModules(result.Battery, Module.Battery);
            FillModules(result.Body, Module.Body);
            FillModules(result.Collector, Module.Collector);
            FillModules(result.CommandCenter, Module.CommandCenter);
            FillModules(result.Converter, Module.Converter);
            FillModules(result.Engine, Module.Engine);
            FillModules(result.Generator, Module.Generator);
            FillModules(result.Gun, Module.Gun);
            FillModules(result.Repairer, Module.Repairer);
            FillModules(result.Storage, Module.Storage);
        }

        private void FillModules<T>(List<T> module, Module moduleType) where T : IBindableModel
        {
            foreach (T item in module)
            {
                modules.Add(item, moduleType);
            }
        }

        private void InitializeBaseModules()
        {
            string content = File.ReadAllText(Constants.BasePath + Constants.BasePiratesDataPath);
            try
            {
                JObject data = (JObject)JsonConvert.DeserializeObject(content);
                JArray bufferResult = (JArray)data["baseModules"];
                baseModules = bufferResult.ToObject<List<Module>>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Json parsing errore");
            }
        }

        private Level GenerateShipLevel()
        {
            Level result = Level.First;
            Random rnd = new Random();
            int value = rnd.Next(0, 1000);
            if(value > 333 && value <= 666)
            {
                result = Level.Second;
            }
            else if (value > 666 && value <= 999)
            {
                result = Level.Third;
            }

            return result;
        }

        private int GenerateNumberOfAdditionalGuns()
        {
            Random rnd = new Random();
            return rnd.Next(0, 4);
        }
    }
}
