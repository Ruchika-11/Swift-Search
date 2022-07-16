using Newtonsoft.Json;

namespace ProductStoreAPI.Response
{
    public class ElasticSearchTotal
    {
        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }

        [JsonProperty(PropertyName = "relation")]
        public string Relation { get; set; }
    }
}
