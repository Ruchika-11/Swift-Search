namespace ProductStoreAPI.Model
{
    public class PriceList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string CurrencyType { get; set; }
        public bool IsActive { get; set; }
    }
}
