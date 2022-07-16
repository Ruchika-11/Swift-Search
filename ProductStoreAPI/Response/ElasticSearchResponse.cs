using Newtonsoft.Json;
using ProductStoreAPI.Model;
using System;
using System.Collections.Generic;

namespace ProductStoreAPI.Response
{
    /// <summary>
    /// Elastic Search Response class
    /// </summary>
    /// <typeparam name="T"> Entity Type to search </typeparam>
    public class ElasticSearchResponse<T> : SearchResponse<T> where T : ProductResponse
    {
        [JsonProperty(PropertyName = "took")]
        public override int TimeTaken { get; set; }

        [JsonProperty(PropertyName = "timed_out")]
        public override bool TimedOut { get; set; }

        [JsonProperty(PropertyName = "hits")]
        public ElasticSearchRecords<T> Records { get; set; }

        [JsonProperty(PropertyName = "error")]
        public ElasticError ErrorDetail { get; set; }

        [JsonProperty(PropertyName = "_scroll_id")]
        public string ScrollId { get; set; }

        [JsonProperty(PropertyName = "aggregations", NullValueHandling = NullValueHandling.Ignore)]
        public ElasticSearchFacets Facets { get; set; }

        /// <summary>
        /// Converts ElasticSearchResponse<BaseEntity> to ElasticSearchResponse<T>
        /// </summary>
        /// <param name="response"> ElasticSearchResponse with BaseEntity </param>
        /// <returns> ElasticSearchResponse with T </returns>
        public ElasticSearchResponse<T> Convert(ElasticSearchResponse<ProductResponse> response)
        {
            TimeTaken = response.TimeTaken;
            TimedOut = response.TimedOut;
            ErrorDetail = response.ErrorDetail;
            ScrollId = response.ScrollId;
            Records = new ElasticSearchRecords<T>();
            Records = Records.Convert(response.Records);
            return this;
        }
    }

    /// <summary>
    /// Elastic Search Response class
    /// </summary>
    /// <typeparam name="T"> Primary Entity Type to search </typeparam>
    /// <typeparam name="U"> Nested Entity Type to search </typeparam>
    public class ElasticSearchResponse<T, U> : SearchResponse<T>      
    {
        [JsonProperty(PropertyName = "took")]
        public override int TimeTaken { get; set; }

        [JsonProperty(PropertyName = "timed_out")]
        public override bool TimedOut { get; set; }

        [JsonProperty(PropertyName = "hits")]
        public ElasticSearchRecords<T, U> Records { get; set; }

        [JsonProperty(PropertyName = "error")]
        public ElasticError ErrorDetail { get; set; }

        [JsonProperty(PropertyName = "_scroll_id")]
        public string ScrollId { get; set; }

        [JsonProperty(PropertyName = "aggregations", NullValueHandling = NullValueHandling.Ignore)]
        public ElasticSearchFacets Facets { get; set; }
    }

}

