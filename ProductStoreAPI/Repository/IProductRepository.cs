using Microsoft.AspNetCore.Mvc;
using ProductStoreAPI.Model;
using ProductStoreAPI.Response;
using System.Collections.Generic;

namespace ProductStoreAPI.Repository
{
    public interface IProductRepository
    {
        Task<ProductFacetResponse> Get(SearchRequest request);
    }
}
