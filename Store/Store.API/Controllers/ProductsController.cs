using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Heleper;
using Store.Repository.Specification.Product;
using Store.Service.HandelResponses;
using Store.Service.Heleper;
using Store.Service.Services.ProductService;
using Store.Service.Services.ProductService.Dtos;

namespace Store.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        [Cache(90)]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllBrands()
       =>Ok( await _productService.GetAllBrandsAsync());

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllTypes()
         => Ok(await _productService.GetAllTypesAsync()); 
        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<ProductDetailsDto>>> GetAllProducts([FromQuery]ProductSpecification input)
        => Ok(await _productService.GetAllProductsAsync(input));
        [HttpGet]
        public async Task<ActionResult<ProductDetailsDto>> GetProduct(int? id)
        {
            if (id is null)
                return BadRequest(new Response(400, "Id Is Null"));
            var product = await _productService.GetProductsByIdAsync(id);
            if (product is null)
                return NotFound(new Response(404));
            return Ok(product);
        }
    }
}

namespace Store.API
{
    class CacheAttribute : Attribute
    {
        public CacheAttribute(int v)
        {
        }
    }
}