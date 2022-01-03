using Space.Model.Constants;
using System.IO;

namespace Space.Infrastructure.Pirates
{
    public class BattlleLogger
    {
        private static object locker;

        static BattlleLogger()
        {
            locker = new object();
        }

        public string LogAction(string actor, string enemy, int damage, int nonActorHpAfterAction, int battleNumber)
        {
            string logLine = string.Empty;
            logLine = $"{actor} caused {damage} damage by {enemy}. {enemy} hp = {nonActorHpAfterAction}\r\n";
            lock (locker)
            {
                File.AppendAllText(Constants.BasePath + Constants.BaseLogPath + battleNumber + ".txt", logLine);
            }

            return logLine;
        }
    }
}
