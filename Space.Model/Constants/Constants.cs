using Space.Model.Enums;
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
        public static int MaximumNumberOfCommandCenter = 1;
        public static int NumberOfModulesInOneBody = 4;

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
        }
    }
}
