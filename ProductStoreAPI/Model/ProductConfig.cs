using System.Collections.Generic;

namespace ProductStoreAPI.Model
{
    public class ProductConfig
    {
        public string Id { get; set; }
        public bool FuzzySearchEnabled { get; set; }
        public List<string> SearchFields { get; set; }
    }
}
