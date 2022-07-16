using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProductStoreAPI.Response
{
    public static class SerializationHelper
    {
        public static JsonSerializer serializer = new JsonSerializer();

        /// <summary>
        /// Only return object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static T GetObject<T>(string values)
        {
            if (!string.IsNullOrEmpty(values))
            {
                var objectData = JObject.Parse(values);
                return serializer.Deserialize<T>(new JTokenReader(objectData));
            }
            return default(T);
        }


        /// <summary>
        /// Only return object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static T GetValues<T>(string values)
        {
            if (!string.IsNullOrEmpty(values))
            {
                var objectData = JArray.Parse(values);
                return serializer.Deserialize<T>(new JTokenReader(objectData));
            }
            return default(T);
        }

    }
}
