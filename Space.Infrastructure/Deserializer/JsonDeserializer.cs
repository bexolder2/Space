using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Space.Helpers.Interfaces;
using Space.Model.Constants;
using Space.Model.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Space.Infrastructure.Deserializer
{
    public class JsonDeserializer
    {
        public static DeserealizationModel InitializeModules()
        {
            var result = new DeserealizationModel();

            result.Battery = DeserializeModule<Battery>(Constants.BasePath + Constants.BattaryDataPath);
            result.Body = DeserializeModule<Body>(Constants.BasePath + Constants.BodyDataPath);
            result.Collector = DeserializeModule<Collector>(Constants.BasePath + Constants.CollectorDataPath);
            result.CommandCenter = DeserializeModule<CommandCenter>(Constants.BasePath + Constants.CommandCenterDataPath);
            result.Converter = DeserializeModule<Converter>(Constants.BasePath + Constants.ConverterDataPath);
            result.Engine = DeserializeModule<Engine>(Constants.BasePath + Constants.EngineDataPath);
            result.Gun = DeserializeModule<Gun>(Constants.BasePath + Constants.GunDataPath);
            result.Storage = DeserializeModule<Storage>(Constants.BasePath + Constants.StorageDataPath);
            result.Generator = DeserializeModule<Generator>(Constants.BasePath + Constants.GeneratorDataPath);
            result.Repairer = DeserializeModule<Repairer>(Constants.BasePath + Constants.RepairerDataPath);

            return result;
        }

        private static List<T> DeserializeModule<T>(string filePath)
        {
            List<T> result = default(List<T>);
            string content = File.ReadAllText(filePath);
            try
            {
                JObject data = (JObject)JsonConvert.DeserializeObject(content);
                JArray bufferResult = (JArray)data[$"{typeof(T).Name}"];
                result = bufferResult.ToObject<List<T>>();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Json parsing errore");
            }

            return result;
        }
    }
}
