using Microsoft.AspNetCore.Mvc;
using ProductStoreAPI.Model;
using ProductStoreAPI.Response;
using Elasticsearch.Net;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;


namespace ProductStoreAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IElasticLowLevelClient _elasticClient;
        public Dictionary<string, List<FacetInfo>> FacetRecords;
        public ProductRepository(IElasticLowLevelClient elasticClient)
        {
            _elasticClient = elasticClient;
            FacetRecords = new Dictionary<string, List<FacetInfo>>() { };
        }
        public async Task<T> SearchAsync<T>(string entityName, string query, bool isTemplate = false)
        {
            try
            {
                StringResponse esResponse = null;

                if (isTemplate)
                {
                    esResponse = await _elasticClient.SearchTemplateAsync<StringResponse>(entityName, PostData.String(query));
                }
                else
                {
                    esResponse = await _elasticClient.SearchAsync<StringResponse>(entityName, PostData.String(query));
                }

                if (!esResponse.Success && esResponse.OriginalException != null)
                {
                    throw esResponse.OriginalException;
                }

                var response = SerializationHelper.GetObject<T>(esResponse.Body);
                return response;
            }
            catch (Exception ex)
            {
                //apttusOpenTracer.LogError<ElasticDataAccessSearchClient>(ex.Message, ex);
                throw;
            }
        }

        public async Task<ProductFacetResponse> Get(SearchRequest request)
        {
            ProductFacetResponse productFacetResponse = new ProductFacetResponse();
            var FROM = (request.PageNumber - 1) * request.PageSize;
            var SIZE = request.PageSize;

            string sort = "{{Sorting_subquery}}";
            var sort_query = @",
                                ""sort"": [
                                {""Product." + request.sort_field + @""": """ + request.sort_order + @"""}
					        ]";

            var Query = @"
            {
                ""from"": """ + FROM + @""",
				""size"": """ + SIZE + @""",
                ""query"": {
                    ""bool"": {
                        ""filter"": [
                            {
                                ""has_child"": {
                                    ""type"": ""ProductCategory"",
                                    ""query"": {
                                        ""term"": {
                                            ""ProductCategory.CategoryId"": """ + request.category_id + @"""
                                        }
                                    }
                                }
                            },
                            {
                            ""has_child"": {
                                ""type"": ""PriceListItem"",
                                ""query"": {
                                    ""term"": {
                                        ""PriceListItem.PriceListId"": """ + request.pricelist_id + @"""
                                        }
                                    }
                                }
                            }
                            {{fuzzy_subquery}}
                            {{facet_subquery}}
                        ]
                    }
                },
                ""aggs"": {
                    {{Aggregation_subquery}}
                }
                {{Sorting_subquery}}
            }";

            

            var product_config_query = @"
            {
	            ""query"": {
		            ""match"": {
			            ""Id"": ""ProductConfig1""
					    }
				  }
            }";
            var config_response = await SearchAsync<ElasticSearchSettingResponse<ProductConfig>>("productsearchconfiguration", product_config_query);
            var fuzzy_config = config_response.Records.ElasticEntities.Select(x => x.Source.FuzzySearchEnabled).ToList();
            bool fuzzy_enabled = false;
            foreach (var choice in fuzzy_config)
            {
                fuzzy_enabled = choice;
            }
            
            var field_config = config_response.Records.ElasticEntities.Select(x => x.Source.SearchFields).ToList();
            List<string> search_fields = new List<string>();
            foreach (var field in field_config)
            {
                foreach (var f in field)
                {
                    search_fields.Add(f);
                }
            }

            //string condition = "{{Search_string_subquery}}";
            string f_names = "{{fields}}";
            var searchstring_query = @",{
                                ""query_string"": {
                                ""query"": ""*" + request.searchstring + @"*"",
                                ""fields"": {{fields}}
					            }
                              }";
            searchstring_query = searchstring_query.Replace(f_names, @"[""" + string.Join(@""",""", search_fields) + @"""]");

            string fuz_fields = "{{FuzzyFields}}";
            string fuzzy_search = "{{fuzzy_subquery}}";
            var fuzzy_query = @",{
                                ""multi_match"": {
                                ""fields"": {{FuzzyFields}},
                                ""query"": """ + request.searchstring + @""",
                                ""fuzziness"": ""2""
					        }
                        }";
            fuzzy_query = fuzzy_query.Replace(fuz_fields, @"[""" + string.Join(@""",""", search_fields) + @"""]");

            if (fuzzy_enabled == true)
            {
                Query = Query.Replace(fuzzy_search, fuzzy_query);
            }
            else if ((fuzzy_enabled == false) && (request.searchstring == null || request.searchstring.Length == 0))
            {
                Query = Query.Replace(fuzzy_search, "");
            }
            else if(fuzzy_enabled == false)
            {
                Query = Query.Replace(fuzzy_search, searchstring_query);
            }

            if (request.sort_field == null || request.sort_field.Length == 0)
            {
                Query = Query.Replace(sort, "");
            }
            else
            {
                Query = Query.Replace(sort, sort_query);
            }
            string facet = "{{facet_subquery}}";
            var facetvalues = "{{Values}}";
            List<string> Facet_Query_List = new List<string>();
            foreach (var facetfield in request.facet_input)
            {
                var facet_query = @",{
                                ""terms"": {
                                ""Product." + facetfield.FacetField + @""": {{Values}}
					            }
                              }";
                facet_query = facet_query.Replace(facetvalues, @"[""" + string.Join(@""",""", facetfield.FacetFieldValue) + @"""]");
                Facet_Query_List.Add(facet_query);
            }
            var fac_Query = string.Join("", Facet_Query_List);
            if (request.facet_input.Count == 0)
            {
                Query = Query.Replace(facet, "");
            }
            else
            {
                Query = Query.Replace(facet, fac_Query);
            }
            
            var CategoryAgg_Query = @"
            {
	            ""query"": {
		            ""match"": {
			            ""Category.Id"": """ + request.category_id + @"""
					    }
				  }
            }";
            var agg_response = await SearchAsync<ElasticSearchResponse<ProductResponse>>("categorystore", CategoryAgg_Query);
            var agg_record = agg_response.Records.ElasticEntities.Select(x => x.Source.Category.FacetFields).ToList();

            int FacetCount;
            List<string> facet_fields = new List<string>();
            foreach (var aggObject in agg_record)
            {
                FacetCount = aggObject.Count;
                facet_fields = aggObject;
            }

            Dictionary<string, string> agg_keyvalue = new Dictionary<string, string>();

            foreach (var facetf in facet_fields)
            {
                var facet_key = facetf + "_options";
                var facet_value = "Product." + facetf;
                agg_keyvalue.Add(facet_key, facet_value);
            }

            var agg = "{{Aggregation_subquery}}";
            List<string> Agg_Query_List = new List<string>();
            
            foreach (var keyvalue in agg_keyvalue)
            {
                var A_Query = @"
                            """ + keyvalue.Key + @""": {
                                ""terms"": {
                                    ""field"": """ + keyvalue.Value + @"""
                            }
					    }";
                Agg_Query_List.Add(A_Query);
            }
            var agg_Query = string.Join(",", Agg_Query_List);
            Query = Query.Replace(agg, agg_Query);

            //Get the time taken for the request to be processsed
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var response = await SearchAsync<ElasticSearchResponse<ProductResponse>>("productstore", Query);
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value. 
            string totalElapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            productFacetResponse.ElasticProcessingTime = response.TimeTaken;
            productFacetResponse.TotalTurnaroundTime = totalElapsedTime;
            productFacetResponse.TotalRecordsCount = response.Records.Total.Value;
            var records = response.Records.ElasticEntities.Select(x => x.Source).ToList();
            var recordresponse = records.Select(x => x.Product).ToList();
            productFacetResponse.Products = recordresponse;
            var product_ids = records.Select(x => x.Product.Id).ToList();
            
            // Adding prices in the product response 
            List<string> list_of_ids = new List<string>();
            foreach (var id in product_ids)
            {
                list_of_ids.Add(id);
            }
            
            string ProductIds = "{{P_ids}}";
            var PLI_Query = @"
            {
	            ""query"": {
		            ""bool"": {
			            ""filter"": [
				            {
				            ""terms"": {
					            ""PriceListItem.ProductId"": {{P_ids}}
					            }
				            },
				            {
				            ""match"": {
					            ""PriceListItem.PriceListId"": """ + request.pricelist_id + @"""
					            }
				            }
			            ]
		            }
	            }
            }";

            PLI_Query = PLI_Query.Replace(ProductIds, @"[""" + string.Join(@""",""", list_of_ids) + @"""]");
            var pli_response = await SearchAsync<ElasticSearchResponse<ProductResponse>>("productstore", PLI_Query);
            var pli_records = pli_response.Records.ElasticEntities.Select(x => x.Source.PriceListItem).ToList();

            Dictionary<string, double> priceListItems = new Dictionary<string, double>();
            if (pli_records != null)
            {
                foreach (var pliObject in pli_records)
                {
                    var id = pliObject.ProductId;
                    var price = pliObject.Price;
                    priceListItems[id] = price;
                }
            }

            foreach(var prod in productFacetResponse.Products)
            {
                if(priceListItems.ContainsKey(prod.Id))
                {
                    prod.Price = priceListItems[prod.Id];
                }
            }

            ProductRepository productRepository = new ProductRepository(_elasticClient);
            productFacetResponse.FacetRecords = productRepository.FacetRecords;

            foreach (var facetItem in response.Facets.Facets)
            {
                ElasticSearchFacet elasticSearchFacet = facetItem.Value as ElasticSearchFacet;
                if (elasticSearchFacet != null)
                {
                    foreach (var item in elasticSearchFacet.FacetsInfo)
                    {
                        FacetInfo facetInfo = new FacetInfo();
                        if (!productFacetResponse.FacetRecords.ContainsKey(facetItem.Key))
                        {
                            productFacetResponse.FacetRecords[facetItem.Key] = new List<FacetInfo>();
                            facetInfo.FacetValue = item.FacetFieldValue;
                            facetInfo.Count = item.RecordsCount;
                            productFacetResponse.FacetRecords[facetItem.Key].Add(facetInfo);
                        }
                        else
                        {
                            facetInfo.FacetValue = item.FacetFieldValue;
                            facetInfo.Count = item.RecordsCount;
                            productFacetResponse.FacetRecords[facetItem.Key].Add(facetInfo);
                        }
                    }
                }
            }
            return productFacetResponse;
        }
    }
}

