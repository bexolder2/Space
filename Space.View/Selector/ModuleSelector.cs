using Space.Helpers.Interfaces;
using Space.Model.Enums;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Space.View.Selector
{
    public class ModuleSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var param = ((KeyValuePair<IBindableModel, Module>)item).Value;
            var element = container as FrameworkElement;
            DataTemplate result = null;

            if (element != null && item != null)
            {
                switch (param)
                {
                    case Module.Battery:
                        result = element.FindResource("Battery") as DataTemplate;
                        break;
                    case Module.Body:
                        result = element.FindResource("Body") as DataTemplate;
                        break;
                    case Module.Collector:
                        result = element.FindResource("Collector") as DataTemplate;
                        break;
                    case Module.CommandCenter:
                        result = element.FindResource("CommandCenter") as DataTemplate;
                        break;
                    case Module.Converter:
                        result = element.FindResource("Converter") as DataTemplate;
                        break;
                    case Module.Engine:
                        result = element.FindResource("Engine") as DataTemplate;
                        break;
                    case Module.Generator:
                        result = element.FindResource("Generator") as DataTemplate;
                        break;
                    case Module.Gun:
                        result = element.FindResource("Gun") as DataTemplate;
                        break;
                    case Module.Repairer:
                        result = element.FindResource("Repairer") as DataTemplate;
                        break;
                    case Module.Storage:
                        result = element.FindResource("Storage") as DataTemplate;
                        break;
                }
            }

            return result;
        }
    }
}
