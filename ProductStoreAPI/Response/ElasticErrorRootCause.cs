using Newtonsoft.Json;

namespace ProductStoreAPI.Response
{
    public class ElasticErrorRootCause
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; set; }

        [JsonProperty(PropertyName = "index_uuid")]
        public string IndexUUID { get; set; }

        [JsonProperty(PropertyName = "index")]
        public string Index { get; set; }

        [JsonProperty(PropertyName = "caused_by")]
        public ElasticErrorCause ErrorDesc { get; set; }
    }
}
