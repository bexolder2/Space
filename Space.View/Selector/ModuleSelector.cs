using Space.Helpers.Interfaces;
using Space.Model.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Space.View.Selector
{
    public class ModuleSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate result = null;

            if (item != null)
            {
                if (item is KeyValuePair<Dictionary<IBindableModel, Module>, int>)
                {
                    var param = ((KeyValuePair<Dictionary<IBindableModel, Module>, int>)item).Key.FirstOrDefault().Value;
                    result = GetTemplate(param, container);
                }
                else if (item is KeyValuePair<IBindableModel, Module>)
                {
                    var param = ((KeyValuePair<IBindableModel, Module>)item).Value;
                    result = GetTemplate(param, container);
                }
            }
            
            return result;
        }

        private DataTemplate GetTemplate(Module param, DependencyObject container)
        {
            DataTemplate result = null;
            var element = container as FrameworkElement;

            switch (param)
            {
                case Module.Battery:
                    result = element.FindResource("Battery") as DataTemplate;
                    break;
                case Module.Body:
                    result = element.FindResource("EmptyBody") as DataTemplate;
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
                case Module.EmptyBody:
                    result = element.FindResource("EmptyBody") as DataTemplate;
                    break;
            }

            return result;
        }
    }
}
