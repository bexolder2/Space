using Space.Helpers.Interfaces;
using Space.Infrastructure.Extensions;
using Space.Model.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Space.Infrastructure.Converters
{
    public class CollectionToUniqueCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var values = value;
            if (value != null && value is Dictionary<IBindableModel, Module>)
            {
                return (value as IDictionary<IBindableModel, Module>).DistinctBy(x => x.Value);
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
