using Newtonsoft.Json;

namespace ProductStoreAPI.Response
{
    public class ElasticShardFailureReason
    {
        [JsonProperty(PropertyName = "Shard")]
        public int Shard { get; set; }

        [JsonProperty(PropertyName = "index")]
        public string Index { get; set; }

        [JsonProperty(PropertyName = "node")]
        public string Node { get; set; }

        [JsonProperty(PropertyName = "reason")]
        public ElasticErrorRootCause Reason { get; set; }
    }
}
