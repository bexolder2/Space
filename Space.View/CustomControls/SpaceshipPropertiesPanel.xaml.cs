using System.Windows;
using System.Windows.Controls;

namespace Space.View.CustomControls
{
    public partial class SpaceshipPropertiesPanel : UserControl
    {
        public SpaceshipPropertiesPanel()
        {
            InitializeComponent();
        }

        #region Dependency properties
        public static readonly DependencyProperty DistanceProperty =
            DependencyProperty.Register("Distance",
                typeof(int),
                typeof(SpaceshipPropertiesPanel));

        public double Distance
        {
            get => (int)GetValue(DistanceProperty);
            set => SetValue(DistanceProperty, value);
        }

        public static readonly DependencyProperty HealthPointProperty =
            DependencyProperty.Register("HealthPoint",
                typeof(int),
                typeof(SpaceshipPropertiesPanel));

        public int HealthPoint
        {
            get => (int)GetValue(HealthPointProperty);
            set => SetValue(HealthPointProperty, value);
        }
        #endregion
    }
}
