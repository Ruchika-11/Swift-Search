namespace ProductStoreAPI.Model
{
    public class Product
    {
        public string Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public double Price { get; set; }
        public string Color { get; set; }
        public string Shape { get; set; }
        public string OperatingSystem { get; set; }
        public string RamSize { get; set; }
        public string InternalStorage { get; set; }
        public bool IsActive { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

    public class PriceListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PriceListId { get; set; }
        public string ProductId { get; set; }
        public double Price { get; set; }
        public string ChargeType { get; set; }
        public bool IsActive { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
    public class ProductCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProductId { get; set; }
        public string CategoryId { get; set; }
    }

    public class ProductResponse
    {
        public Product Product { get; set; }
        public PriceListItem PriceListItem { get; set; }
        public Category Category { get; set; }
    }
}