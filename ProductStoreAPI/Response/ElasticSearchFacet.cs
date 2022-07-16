using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductStoreAPI.Response
{
    public class ElasticSearchFacet
    {
        [JsonProperty(PropertyName = "doc_count_error_upper_bound")]
        public int DocumentCountError { get; set; }

        [JsonProperty(PropertyName = "sum_other_doc_count")]
        public int SumDocumentCount { get; set; }

        [JsonProperty(PropertyName = "buckets")]
        public List<ElasticSearchFacetInfo> FacetsInfo { get; set; }

        public ElasticSearchFacet()
        {
            FacetsInfo = new List<ElasticSearchFacetInfo>();
        }
    }
}
