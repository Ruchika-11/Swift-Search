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
    public class ElasticSearchSettingResponse<T> : SearchResponse<T> where T : ProductConfig
    {
        [JsonProperty(PropertyName = "took")]
        public override int TimeTaken { get; set; }

        [JsonProperty(PropertyName = "timed_out")]
        public override bool TimedOut { get; set; }

        [JsonProperty(PropertyName = "hits")]
        public ElasticSearchSettingRecords<T> Records { get; set; }

        [JsonProperty(PropertyName = "error")]
        public ElasticError ErrorDetail { get; set; }

        [JsonProperty(PropertyName = "_scroll_id")]
        public string ScrollId { get; set; }

        [JsonProperty(PropertyName = "aggregations", NullValueHandling = NullValueHandling.Ignore)]
        public ElasticSearchFacets Facets { get; set; }

        /// <summary>
        /// Converts ElasticSearchSettingResponse<BaseEntity> to ElasticSearchSettingResponse<T>
        /// </summary>
        /// <param name="response"> ElasticSearchSettingResponse with BaseEntity </param>
        /// <returns> ElasticSearchSettingResponse with T </returns>
        public ElasticSearchSettingResponse<T> Convert(ElasticSearchSettingResponse<ProductConfig> response)
        {
            TimeTaken = response.TimeTaken;
            TimedOut = response.TimedOut;
            ErrorDetail = response.ErrorDetail;
            ScrollId = response.ScrollId;
            Records = new ElasticSearchSettingRecords<T>();
            Records = Records.Convert(response.Records);
            return this;
        }
    }

    /// <summary>
    /// Elastic Search Response class
    /// </summary>
    /// <typeparam name="T"> Primary Entity Type to search </typeparam>
    /// <typeparam name="U"> Nested Entity Type to search </typeparam>
    public class ElasticSearchSettingResponse<T, U> : SearchResponse<T>
    {
        [JsonProperty(PropertyName = "took")]
        public override int TimeTaken { get; set; }

        [JsonProperty(PropertyName = "timed_out")]
        public override bool TimedOut { get; set; }

        [JsonProperty(PropertyName = "hits")]
        public ElasticSearchSettingRecords<T, U> Records { get; set; }

        [JsonProperty(PropertyName = "error")]
        public ElasticError ErrorDetail { get; set; }

        [JsonProperty(PropertyName = "_scroll_id")]
        public string ScrollId { get; set; }

        [JsonProperty(PropertyName = "aggregations", NullValueHandling = NullValueHandling.Ignore)]
        public ElasticSearchFacets Facets { get; set; }
    }

}



