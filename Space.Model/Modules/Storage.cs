namespace Space.Model.Modules
{
    public class Storage : BaseModel
    {
        private int limit;

        #region properties
        public int Limit
        {
            get => limit;
            set => Set(ref limit, value);
        }
        #endregion
    }
}
