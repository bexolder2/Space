namespace Space.Model.Modules
{
    public class Body : BaseModel
    {
        private int index;

        #region properties
        public int Index
        {
            get => index;
            set => Set(ref index, value);
        }
        #endregion
    }
}
