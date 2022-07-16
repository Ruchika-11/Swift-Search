using Newtonsoft.Json;

namespace ProductStoreAPI.Response
{
    public class ElasticErrorCause
    {
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorType { get; set; }

        [JsonProperty(PropertyName = "reason", NullValueHandling = NullValueHandling.Ignore)]
        public string Reason { get; set; }

        [JsonProperty(PropertyName = "line", NullValueHandling = NullValueHandling.Ignore)]
        public int QueryLineNumber { get; set; }

        [JsonProperty(PropertyName = "col", NullValueHandling = NullValueHandling.Ignore)]
        public int QueryColumn { get; set; }

        [JsonProperty(PropertyName = "index_uuid", NullValueHandling = NullValueHandling.Ignore)]
        public string IndexUuid { get; set; }

        [JsonProperty(PropertyName = "index", NullValueHandling = NullValueHandling.Ignore)]
        public string Index { get; set; }
    }
}
