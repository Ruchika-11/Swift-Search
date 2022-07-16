using Newtonsoft.Json;
using ProductStoreAPI.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductStoreAPI.Response
{
    [JsonConverter(typeof(AggregatedRecordsSerializer))]
    public class ElasticSearchFacetInfo
    {
        [JsonProperty(PropertyName = "key")]
        public string FacetFieldValue { get; set; }

        [JsonProperty(PropertyName = "doc_count")]
        public long RecordsCount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, ElasticSearchResponse<ProductResponse>> Records { get; set; }
    }
}
