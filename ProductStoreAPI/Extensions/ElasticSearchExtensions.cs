using Elasticsearch.Net;

namespace ProductStoreAPI.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticSearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            //var defaultindex = configuration["elasticsearch:index"];

            var settings = new ConnectionConfiguration(new Uri("http://localhost:9200")).ThrowExceptions();
            var client = new ElasticLowLevelClient(settings);
            services.AddSingleton<IElasticLowLevelClient>(client); 
        }
        
    }
}
