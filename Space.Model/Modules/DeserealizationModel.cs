using Newtonsoft.Json;
using System.Collections.Generic;

namespace Space.Model.Modules
{
    public class DeserealizationModel
    {
        public List<Battery> Battery { get; set; }
        public List<Body> Body { get; set; }
        public List<Collector> Collector { get; set; }
        public List<CommandCenter> CommandCenter { get; set; }
        public List<Converter> Converter { get; set; }
        public List<Engine> Engine { get; set; }
        public List<Gun> Gun { get; set; }
        public List<Storage> Storage { get; set; }
        public List<Generator> Generator { get; set; }
        public List<Repairer> Repairer { get; set; }

        public DeserealizationModel()
        {
            Battery = new List<Battery>();
            Body = new List<Body>();
            Collector = new List<Collector>();
            CommandCenter = new List<CommandCenter>();
            Converter = new List<Converter>();
            Engine = new List<Engine>();
            Gun = new List<Gun>();
            Storage = new List<Storage>();
            Generator = new List<Generator>();
            Repairer = new List<Repairer>();
        }
    }
}
