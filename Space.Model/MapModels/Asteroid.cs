using Space.Helpers;
using System.Windows;

namespace Space.Model.Modules
{
    public class Asteroid : BindableBase.BindableBase
    {
        private Point coordinates;
        private string name;
        private int numberOfOre;

        #region properties
        public Point Coordinates
        {
            get => coordinates;
            set => Set(ref coordinates, value);
        }

        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        public int NumberOfOre
        {
            get => numberOfOre;
            set => Set(ref numberOfOre, value);
        }
        #endregion

        public Asteroid()
        {
            name = new NameGenerator().Generate();
            numberOfOre = new OreGenerator().Generate();
        }
    }
}
