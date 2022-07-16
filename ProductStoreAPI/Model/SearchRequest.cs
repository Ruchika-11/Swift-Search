namespace ProductStoreAPI.Model
{
    public class SearchRequest
    {
        public string category_id { get; set; }
        public string pricelist_id { get; set; }
        public string searchstring { get; set; }
        public string sort_field { get; set; }
        public string sort_order { get; set; }
        public List<FacetInput> facet_input { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
