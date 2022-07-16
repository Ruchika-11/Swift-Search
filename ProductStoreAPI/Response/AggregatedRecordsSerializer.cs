using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using ProductStoreAPI.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;

namespace ProductStoreAPI.Response
{
    public class AggregatedRecordsSerializer : JsonConverter
    {
        string RecordsPropertyName = "Records";
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


            JToken tokenData = JObject.Parse(writer.ToString());

            List<string> propertiesToRemove = new List<string>();

            var properties = target.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (!property.Name.Equals(RecordsPropertyName))
                {
                    var customAttrs = (JsonPropertyAttribute)property.GetCustomAttribute(typeof(JsonPropertyAttribute));
                    if (!string.IsNullOrEmpty(customAttrs.PropertyName))
                    {
                        var propType = property.PropertyType;
                        var value = tokenData[customAttrs.PropertyName];
                        property.SetValue(target, Convert.ChangeType(value, propType));
                        propertiesToRemove.Add(customAttrs.PropertyName);
                    }
                }
            }

            RemoveFields(tokenData, propertiesToRemove);

            var recordsProperty = target.GetType().GetProperty(RecordsPropertyName);
            var records = tokenData.HasValues ? tokenData.ToObject<Dictionary<string, ElasticSearchResponse<ProductResponse>>>() : null;
            recordsProperty.SetValue(target, records);
            return target;
        }

        private void RemoveFields(JToken token, List<string> fields)
        {
            JContainer container = token as JContainer;
            if (container == null) return;

            List<JToken> removeList = new List<JToken>();
            foreach (JToken el in container.Children())
            {
                JProperty p = el as JProperty;
                if (p != null && fields.Contains(p.Name))
                {
                    removeList.Add(el);
                }
                RemoveFields(el, fields);
            }

            foreach (JToken el in removeList)
            {
                el.Remove();
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.PreserveReferencesHandling = PreserveReferencesHandling.None;
            var expandoObj = new ExpandoObject() as IDictionary<string, Object>;
            var properties = value.GetType().GetProperties();
            foreach (var property in properties)
            {
                var customAttrs = (JsonPropertyAttribute)property.GetCustomAttribute(typeof(JsonPropertyAttribute));
                if (property.GetValue(value) != null || (property.GetValue(value) == null && customAttrs.NullValueHandling != NullValueHandling.Ignore))
                {
                    var propertyName = !string.IsNullOrEmpty(customAttrs.PropertyName) ? customAttrs.PropertyName : property.Name;
                    expandoObj.Add(propertyName, property.GetValue(value));
                }
            }
            serializer.Serialize(writer, expandoObj);
        }
    }
}
