namespace HappyTokenApi.Models
{
    public class StoreCurrencySpot
    {
        public CurrencyType BuyCurrencyType { get; set; }

        public int BuyAmount { get; set; }

        public CurrencyType SellCurrencyType { get; set; }

        public int SellAmount{ get; set; }

        public bool IsVisible { get; set; }

        public bool IsPromoted { get; set; }
    }
}
