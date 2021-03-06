using Newtonsoft.Json;
using ProductStoreAPI.Model;
using System.Reflection;

namespace ProductStoreAPI.Response
{
    public class ElasticSearchSettingObject<T> where T : ProductConfig
    {
        [JsonProperty(PropertyName = "_index")]
        public string IndexName { get; set; }

        [JsonProperty(PropertyName = "_doc")]
        public string Document { get; set; }

        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "_score")]
        public double? Score { get; set; }

        [JsonProperty(PropertyName = "_source")]
        public T Source { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public ElasticSearchFields Fields { get; set; }

        [JsonProperty(PropertyName = "sort")]
        public List<object> SortValues { get; set; }


        /// <summary>
        /// Converts ElasticSearchSettingObject<BaseEntity> to ElasticSearchSettingObject<T>
        /// </summary>
        /// <param name="entity"> ElasticSearchSettingObject with BaseEntity </param>
        /// <returns> ElasticSearchSettingObject with T </returns>
        public ElasticSearchSettingObject<T> Convert(ElasticSearchSettingObject<ProductConfig> entity)
        {
            IndexName = entity.IndexName;
            Document = entity.Document;
            Id = entity.Id;
            Score = entity.Score;
            Fields = entity.Fields;
            SortValues = entity.SortValues;
            Source = (T)Activator.CreateInstance(typeof(T), new object[] { ObjectToDictionary(entity.Source) });
            return this;
        }
        public static Dictionary<string, object> ObjectToDictionary(object obj)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                string propName = prop.Name;
                var val = obj.GetType().GetProperty(propName).GetValue(obj, null);
                if (val != null)
                {
                    ret.Add(propName, val);
                }
                else
                {
                    ret.Add(propName, null);
                }
            }
            return ret;
        }
    }
    /// <summary>
    /// ElasticSearch Object class
    /// </summary>
    /// <typeparam name="T"> Primary Entity Type to search </typeparam>
    /// <typeparam name="U"> Nested Entity Type to search </typeparam>
    public class ElasticSearchSettingObject<T, U>

    {
        [JsonProperty(PropertyName = "_index")]
        public string IndexName { get; set; }

        [JsonProperty(PropertyName = "_doc")]
        public string Document { get; set; }

        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "_score")]
        public double? Score { get; set; }

        [JsonProperty(PropertyName = "_source")]
        public T Source { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public ElasticSearchFields Fields { get; set; }

        [JsonProperty(PropertyName = "sort")]
        public List<object> SortValues { get; set; }
    }
}
