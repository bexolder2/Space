namespace Space.Model.Modules
{
    public class Engine : BaseModel
    {
        private int energyConsymptionPerBattle;
        private int energyConsymptionPer100Kilometers;

        #region properties
        public int EnergyConsymptionPerBattle
        {
            get => energyConsymptionPerBattle;
            set => Set(ref energyConsymptionPerBattle, value);
        }

        public int EnergyConsymptionPer100Kilometers
        {
            get => energyConsymptionPer100Kilometers;
            set => Set(ref energyConsymptionPer100Kilometers, value);
        }
        #endregion
    }
}
