using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductStoreAPI.Response
{
    [JsonConverter(typeof(FacetsSerializer))]
    public class ElasticSearchFacets
    {
        public ElasticSearchFacets()
        {
            Facets = new Dictionary<string, ElasticSearchFacet>();
        }

        public Dictionary<string, ElasticSearchFacet> Facets { get; set; }
    }
}
