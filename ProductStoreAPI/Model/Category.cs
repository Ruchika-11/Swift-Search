using System.Collections.Generic;

namespace ProductStoreAPI.Model
{
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> FacetFields { get; set; }
    }

    public class PriceListCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PriceListId { get; set; }
        public string CategoryId { get; set; }
    }

}
