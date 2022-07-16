using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStoreAPI.Model;
using ProductStoreAPI.Repository;
using ProductStoreAPI.Response;

namespace ProductStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Search Product based on search request
        // Post: api/<ProductsController>
        [HttpPost]
        public async Task<ProductFacetResponse> Get(SearchRequest request)
        {
            return await _productRepository.Get(request);
        }
    }
}
