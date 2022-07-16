using Newtonsoft.Json;

namespace ProductStoreAPI.Response
{
    public class ElasticError
    {
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorType { get; set; }

        [JsonProperty(PropertyName = "reason", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorReason { get; set; }

        [JsonProperty(PropertyName = "caused_by", NullValueHandling = NullValueHandling.Ignore)]
        public ElasticErrorCause ErrorDesc { get; set; }

        [JsonProperty(PropertyName = "root_cause", NullValueHandling = NullValueHandling.Ignore)]
        public List<ElasticErrorRootCause> ErrorRootCause { get; set; }

        [JsonProperty(PropertyName = "failed_shards", NullValueHandling = NullValueHandling.Ignore)]
        public List<ElasticShardFailureReason> SharedFailureReason { get; set; }
    }
}
