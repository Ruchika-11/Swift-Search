namespace ProductStoreAPI.Model
{
    public class ProductFacetResponse
    {
        public int ElasticProcessingTime { get; set; }
        public string TotalTurnaroundTime { get; set; }
        public int TotalRecordsCount { get; set; }
        public List<Product>? Products { get; set; }
        public Dictionary<string, List<FacetInfo>> FacetRecords { get; set; }
    }
}
