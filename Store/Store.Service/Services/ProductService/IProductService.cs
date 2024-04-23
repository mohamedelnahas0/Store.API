using Store.Repository.Specification.Product;
using Store.Service.Heleper;
using Store.Service.Services.ProductService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.ProductService
{
    public interface IProductService
    {
        Task<ProductDetailsDto> GetProductByIdAsync(int? id);
        Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsAsync(ProductSpecification input);
        Task<IReadOnlyList<ProductDetailsDto>> GetAllBrandsAsync();

        Task<IReadOnlyList<ProductDetailsDto>> GetAllTypesAsync();
        Task<object?> GetProductsByIdAsync(int? id);
    }
}
