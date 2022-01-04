﻿namespace Space.Model.Entities
{
    public class MarketModel : BindableBase.BindableBase
    {
        private int value;
        private double price;
        private bool isDelivery;

        #region properties
        public int Value
        {
            get => value;
            set => Set(ref this.value, value);
        }

        public double Price
        {
            get => price;
            set => Set(ref price, value);
        }

        public bool IsDelivery
        {
            get => isDelivery;
            set => Set(ref isDelivery, value);
        }
        #endregion
    }
}
