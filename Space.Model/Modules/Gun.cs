namespace Space.Model.Modules
{
    public class Gun : BaseModel
    {
        private int damage;

        #region properties
        public int Damage
        {
            get => damage;
            set => Set(ref damage, value);
        }
        #endregion
    }
}
