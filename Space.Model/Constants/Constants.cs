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
    }
}
