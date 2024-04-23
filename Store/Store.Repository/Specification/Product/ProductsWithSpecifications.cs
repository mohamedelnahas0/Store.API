﻿namespace Store.Repository.Specification.Product
{
    public class ProductsWithSpecifications : BaseSpecification<Data.Entities.Product>
    {
        public ProductsWithSpecifications(ProductSpecification specs)
            :base(
                 product =>(!specs.BrandeId.HasValue || product.BrandId == specs.BrandeId.Value) &&
                             (!specs.TypeId.HasValue || product.TypeId == specs.TypeId.Value )&&
                             (string.IsNullOrEmpty(specs.Search) || product.Name.Trim().ToLower().Contains(specs.Search))
                 )
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.Type);
            AddOrderBy(x => x.Name);

            ApplyPagination(specs.PageSize *(specs.PageIndex-1),specs.PageSize);
            if(!string.IsNullOrEmpty(specs.Sort))
            {
                switch (specs.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(x => x.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(x => x.Price);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }
        }
        public ProductsWithSpecifications(int? id): base(product => product.Id == id)
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.Type);
        }
    }
}
