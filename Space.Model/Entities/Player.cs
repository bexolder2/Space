namespace Space.Model.Modules
{
    public class Player : Actor
    {
        private ResourcesBase resources;

        #region properties
        public ResourcesBase Resources
        {
            get => resources;
            set => Set(ref resources, value);
        }
        #endregion
    }
}
