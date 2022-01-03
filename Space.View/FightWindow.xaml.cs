using System.Windows;

namespace Space.View
{
    public partial class FightWindow : Window
    {
        public FightWindow()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
