using Newtonsoft.Json;
using ProductStoreAPI.Model;

namespace ProductStoreAPI.Response
{
    public class ElasticSearchSettingRecords<T> where T : ProductConfig
    {
        [JsonProperty(PropertyName = "total")]
        public ElasticSearchTotal Total { get; set; }

        [JsonProperty(PropertyName = "max_score")]
        public double? MaxScore { get; set; }

        [JsonProperty(PropertyName = "hits")]
        public List<ElasticSearchSettingObject<T>> ElasticEntities { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        /// <summary>
        /// Converts ElasticSearchSettingRecords<BaseEntity> to ElasticSearchSettingRecords<T>
        /// </summary>
        /// <param name="records"> ElasticSearchSettingRecords with BaseEntity </param>
        /// <returns> ElasticSearchSettingRecords with T </returns>
        public ElasticSearchSettingRecords<T> Convert(ElasticSearchSettingRecords<ProductConfig> records)
        {
            Total = records.Total;
            MaxScore = records.MaxScore;
            Status = records.Status;
            if (records.ElasticEntities?.Any() == true)
            {
                ElasticEntities = new List<ElasticSearchSettingObject<T>>();
                records.ElasticEntities.ForEach(rec =>
                {
                    ElasticSearchSettingObject<T> currEntity = new ElasticSearchSettingObject<T>();
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
    public class ElasticSearchSettingRecords<T, U>

    {
        [JsonProperty(PropertyName = "total")]
        public ElasticSearchTotal Total { get; set; }

        [JsonProperty(PropertyName = "max_score")]
        public double? MaxScore { get; set; }

        [JsonProperty(PropertyName = "hits")]
        public List<ElasticSearchSettingObject<T, U>> ElasticEntities { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
    }
}


