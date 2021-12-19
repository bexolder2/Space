namespace Space.Model.Modules
{
    public class Collector : BaseModel
    {
        private int collectPerCruise;

        #region properties
        public int CollectPerCruise
        {
            get => collectPerCruise;
            set => Set(ref collectPerCruise, value);
        }
        #endregion
    }
}
