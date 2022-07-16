using Newtonsoft.Json;
using ProductStoreAPI.Model;

namespace ProductStoreAPI.Response
{
    public class ElasticSearchRecords<T> where T : ProductResponse
    {
        [JsonProperty(PropertyName = "total")]
        public ElasticSearchTotal Total { get; set; }

        [JsonProperty(PropertyName = "max_score")]
        public double? MaxScore { get; set; }

        [JsonProperty(PropertyName = "hits")]
        public List<ElasticSearchObject<T>> ElasticEntities { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        /// <summary>
        /// Converts ElasticSearchRecords<BaseEntity> to ElasticSearchRecords<T>
        /// </summary>
        /// <param name="records"> ElasticSearchRecords with BaseEntity </param>
        /// <returns> ElasticSearchRecords with T </returns>
        public ElasticSearchRecords<T> Convert(ElasticSearchRecords<ProductResponse> records)
        {
            Total = records.Total;
            MaxScore = records.MaxScore;
            Status = records.Status;
            if (records.ElasticEntities?.Any() == true)
            {
                ElasticEntities = new List<ElasticSearchObject<T>>();
                records.ElasticEntities.ForEach(rec =>
                {
                    ElasticSearchObject<T> currEntity = new ElasticSearchObject<T>();
                    ElasticEntities.Add(currEntity.Convert(rec));
                });
            }
            return this;
        }
    }

    /// <summary>
    /// Elastic Search Records class
    /// </summary>
    /// <typeparam name="T"> Primary Entity Type to search </typeparam>
    /// <typeparam name="U"> Nested Entity Type to search </typeparam>
    public class ElasticSearchRecords<T, U>
            
    {
        [JsonProperty(PropertyName = "total")]
        public ElasticSearchTotal Total { get; set; }

        [JsonProperty(PropertyName = "max_score")]
        public double? MaxScore { get; set; }

        [JsonProperty(PropertyName = "hits")]
        public List<ElasticSearchObject<T, U>> ElasticEntities { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
    }
}

