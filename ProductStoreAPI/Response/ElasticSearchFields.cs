using Newtonsoft.Json;

namespace ProductStoreAPI.Response
{
    public class ElasticSearchFields
    {
        [JsonProperty(PropertyName = "_percolator_document_slot")]
        public List<int> PercolatorDocumentSlot { get; set; }
    }
}
