using Space.Helpers.Interfaces;
using Space.Model.Enums;
using Space.Model.Modules;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Space.Infrastructure.Converters
{
    public class IBindableModelToModelTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            KeyValuePair<IBindableModel, Module> model = (KeyValuePair<IBindableModel, Module>)value;
            StringBuilder result = new StringBuilder();
            switch (model.Value)
            {
                case Module.Battery:
                    {
                        var battery = (Battery)model.Key;
                        result.Append("\nLimit: ");
                        result.Append(battery.Limit);
                    } 
                    break;
                case Module.Body:
                    {
                        var body = (Body)model.Key;
                        result.Append("-");
                    }
                    break;
                case Module.Collector:
                    {
                        var collector = (Collector)model.Key;
                        result.Append("\nOre per cruice: ");
                        result.Append(collector.CollectPerCruise);
                    }
                    break;
                case Module.CommandCenter:
                    {
                        var command = (CommandCenter)model.Key;
                        result.Append("\nBody limit: ");
                        result.Append(command.BodyLimit);
                    }
                    break;
                case Module.Converter:
                    {
                        var converter = (Converter)model.Key;
                        //TODO: conv data
                    }
                    break;
                case Module.Engine:
                    {
                        var engine = (Engine)model.Key;
                        result.Append("\nEnergy consymption per battle: ");
                        result.Append(engine.EnergyConsymptionPerBattle);
                        result.Append("\nEnergy consymption per 100 km: ");
                        result.Append(engine.EnergyConsymptionPer100Kilometers);
                    }
                    break;
                case Module.Generator:
                    {
                        var generator = (Generator)model.Key;
                        //TODO: generator data
                    }
                    break;
                case Module.Gun:
                    {
                        var gun = (Gun)model.Key;
                        result.Append("\nDamage: ");
                        result.Append(gun.Damage);
                    }
                    break;
                case Module.Repairer:
                    {
                        var repairer = (Repairer)model.Key;
                        //TODO: repairer data
                    }
                    break;
                case Module.Storage:
                    {
                        var storage = (Storage)model.Key;
                        result.Append("\nOre limit: ");
                        result.Append(storage.Limit);
                    }
                    break;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
