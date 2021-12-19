namespace Space.Model.Modules
{
    public class CommandCenter : BaseModel
    {
        private int bodyLimit;

        #region properties
        public int BodyLimit
        {
            get => bodyLimit;
            set => Set(ref bodyLimit, value);
        }
        #endregion
    }
}
