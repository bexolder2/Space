using Space.Helpers.Interfaces;
using Space.Model.Enums;
using Space.Model.Modules;
using System.Collections.Generic;
using System.IO;

namespace Space.Model.Constants
{
    public class Constants
    {
        public static readonly string BasePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;

        public const string BattaryDataPath = "\\Space\\Space.Model\\Data\\Battery.json";
        public const string BodyDataPath = "\\Space\\Space.Model\\Data\\Body.json";
        public const string CollectorDataPath = "\\Space\\Space.Model\\Data\\Collector.json";
        public const string CommandCenterDataPath = "\\Space\\Space.Model\\Data\\CommandCenter.json";
        public const string ConverterDataPath = "\\Space\\Space.Model\\Data\\Converter.json";
        public const string EngineDataPath = "\\Space\\Space.Model\\Data\\Engine.json";
        public const string GunDataPath = "\\Space\\Space.Model\\Data\\Gun.json";
        public const string StorageDataPath = "\\Space\\Space.Model\\Data\\Storage.json";
        public const string GeneratorDataPath = "\\Space\\Space.Model\\Data\\Generator.json";
        public const string RepairerDataPath = "\\Space\\Space.Model\\Data\\Repairer.json";

        public static Dictionary<Module, int> MinimalShipConfiguration { get; private set; }
        public static List<KeyValuePair<IBindableModel, Module>> BaseComplectation { get; private set; }
        public static int MaximumNumberOfCommandCenter = 1;
        public static int NumberOfModulesInOneBody = 4;
        public static List<List<bool>> ModulesLocationValidator;


        static Constants()
        {
            MinimalShipConfiguration = new Dictionary<Module, int>();
            MinimalShipConfiguration.Add(Module.CommandCenter, 1);
            MinimalShipConfiguration.Add(Module.Battery, 1);
            MinimalShipConfiguration.Add(Module.Storage, 1);
            MinimalShipConfiguration.Add(Module.Gun, 1);
            MinimalShipConfiguration.Add(Module.Collector, 1);
            MinimalShipConfiguration.Add(Module.Converter, 1);
            MinimalShipConfiguration.Add(Module.Body, 2);
            MinimalShipConfiguration.Add(Module.Engine, 1);

            BaseComplectation = new List<KeyValuePair<IBindableModel, Module>>();
            BaseComplectation.Add(new KeyValuePair<IBindableModel, Module>(new CommandCenter { BodyLimit = 4, HP = 10, Level = Level.First, Price = 100}, Module.CommandCenter));
            BaseComplectation.Add(new KeyValuePair<IBindableModel, Module>(new Battery { HP = 10, Level = Level.First, Price = 150, Limit = 1000000 }, Module.Battery));
            BaseComplectation.Add(new KeyValuePair<IBindableModel, Module>(new Gun { HP = -5, Level = Level.First, Price = 150, Damage = 50 }, Module.Gun));
            BaseComplectation.Add(new KeyValuePair<IBindableModel, Module>(new Storage { HP = 10, Level = Level.First, Price = 50, Limit = 2000 }, Module.Storage));         
            BaseComplectation.Add(new KeyValuePair<IBindableModel, Module>(new Collector { HP = 10, Level = Level.First, Price = 75, CollectPerCruise = 20 }, Module.Collector));
            BaseComplectation.Add(new KeyValuePair<IBindableModel, Module>(new EmptyBody { Level = Level.First, Price = 0 }, Module.EmptyBody));
            BaseComplectation.Add(new KeyValuePair<IBindableModel, Module>(new EmptyBody { Level = Level.First, Price = 0 }, Module.EmptyBody));
            BaseComplectation.Add(new KeyValuePair<IBindableModel, Module>(new Engine { HP = -10, Level = Level.First, Price = 200,
                EnergyConsymptionPer100Kilometers = 50, EnergyConsymptionPerBattle = 10 }, Module.Engine));          
            BaseComplectation.Add(new KeyValuePair<IBindableModel, Module>(new Body { HP = 100, Level = Level.First, Price = 100 }, Module.Body));
            BaseComplectation.Add(new KeyValuePair<IBindableModel, Module>(new Body { HP = 100, Level = Level.First, Price = 100 }, Module.Body));

            ModulesLocationValidator = new List<List<bool>>
            {
                new List<bool> { true, true, false, true, false, false, true, true, true, true, true },
                new List<bool> { true, true, true, true, true, true, true, true, true, true, true },
                new List<bool> { false, true, true, true, true, false, false, false, true, true, true },
                new List<bool> { true, true, true, true, false, true, true, true, true, true, true },
                new List<bool> { false, true, true, false, true, true, true, true, true, true, true },
                new List<bool> { false, true, false, true, true, true, false, false, false, true, true },
                new List<bool> { true, true, false, true, true, false, true, true, true, true, true },
                new List<bool> { true, true, false, true, true, false, true, true, true, true, true },
                new List<bool> { true, true, true, true, true, false, true, true, true, true, true },
                new List<bool> { true, true, true, true, true, true, true, true, true, true, true },
                new List<bool> { true, true, true, true, true, true, true, true, true, true, true }
            };
        }
    }
}
