using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;

namespace ProductStoreAPI.Response
{
    public class FacetsSerializer : JsonConverter
    {
        private const string FacetsPropertyName = "Facets";
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ElasticSearchFacets);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            StringWriter writer = new StringWriter();
            serializer.NullValueHandling = NullValueHandling.Include;
            serializer.DefaultValueHandling = DefaultValueHandling.Include;
            serializer.PreserveReferencesHandling = PreserveReferencesHandling.None;
            JObject jObject = JObject.Load(reader);
            serializer.Serialize(writer, jObject);
            var target = Activator.CreateInstance(objectType);
            var facetsProperty = target.GetType().GetProperty(FacetsPropertyName);
            var facets = JObject.Parse(writer.ToString()).ToObject<Dictionary<string, ElasticSearchFacet>>();
            facetsProperty.SetValue(target, facets);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PropertyInfo facetsProperty = value.GetType().GetProperty(FacetsPropertyName);
            var obj = facetsProperty.GetValue(value);

            serializer.PreserveReferencesHandling = PreserveReferencesHandling.None;
            serializer.Serialize(writer, obj);
        }
    }
}
