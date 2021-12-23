namespace Space.Model.Modules
{
    public abstract class Actor : BindableBase.BindableBase
    {
        private Spaceship spaceship;

        #region properties
        public Spaceship Spaceship
        {
            get => spaceship;
            set => Set(ref spaceship, value);
        }
        #endregion
    }
}
