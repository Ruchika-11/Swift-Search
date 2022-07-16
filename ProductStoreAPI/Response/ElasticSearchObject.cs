using Newtonsoft.Json;
using ProductStoreAPI.Model;
using System.Reflection;

namespace ProductStoreAPI.Response
{
    public class ElasticSearchObject<T> where T : ProductResponse
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
        /// Converts ElasticSearchObject<BaseEntity> to ElasticSearchObject<T>
        /// </summary>
        /// <param name="entity"> ElasticSearchObject with BaseEntity </param>
        /// <returns> ElasticSearchObject with T </returns>
        public ElasticSearchObject<T> Convert(ElasticSearchObject<ProductResponse> entity)
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
    public class ElasticSearchObject<T, U>
            
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

        //[JsonProperty(PropertyName = "inner_hits")]
        //public ElasticSearchNestedRecords<U> NestedRecords { get; set; }
    }
}

