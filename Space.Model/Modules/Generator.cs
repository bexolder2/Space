namespace Space.Model.Modules
{
    public class Generator : BaseModel
    {
        private Efficiency efficiency;

        #region properties
        public Efficiency Efficiency
        {
            get => efficiency;
            set => Set(ref efficiency, value);
        }
        #endregion
    }
}
