using Space.Model.Enums;
using Space.Model.Modules;
using System.Windows;
using System.Windows.Controls;

namespace Space.View.Selector
{
    public class TemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var param = (item as Cell).CellType;
            var element = container as FrameworkElement;
            DataTemplate result = null;

            if (element != null && item != null)
            {
                switch (param)
                {
                    case CellType.Empty:
                        result = element.FindResource("EmptyCell") as DataTemplate;
                        break;
                    case CellType.Station:
                        result = element.FindResource("StationCell") as DataTemplate;
                        break;
                    case CellType.Planet1:
                        result = element.FindResource("Planet1Cell") as DataTemplate;
                        break;
                    case CellType.Planet2:
                        result = element.FindResource("Planet2Cell") as DataTemplate;
                        break;
                    case CellType.Asteroid:
                        result = element.FindResource("AsteroidCell") as DataTemplate;
                        break;
                    case CellType.Player:
                        result = element.FindResource("PlayerCell") as DataTemplate;
                        break;
                    case CellType.PlayerAndAsteroid:
                        result = element.FindResource("PlayerAndAsteroidCell") as DataTemplate;
                        break;
                    case CellType.PlayerAndStation:
                        result = element.FindResource("PlayerAndStationCell") as DataTemplate;
                        break;
                    case CellType.PlayerAndPlanet1:
                        result = element.FindResource("PlayerAndPlanet1Cell") as DataTemplate;
                        break;
                    case CellType.PlayerAndPlanet2:
                        result = element.FindResource("PlayerAndPlanet2Cell") as DataTemplate;
                        break;
                }
            }     

            return result;
        }
    }
}
