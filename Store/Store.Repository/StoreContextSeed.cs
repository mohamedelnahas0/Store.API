using Microsoft.Extensions.Logging;
using Store.Data.Context;
using Store.Data.Entities;
using System.Text.Json;

namespace Store.Repository
{
    public class StoreContextSeed
    {
public static async Task SeedAsyns(StoreDbContext context ,ILoggerFactory  loggerFactory)
        {
            try
            {
                if (context.ProductBrands != null && !context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Store.Repository/SeedData/brands.json");

                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    if(brands is not null)
                    
                            await context.ProductBrands.AddRangeAsync(brands);
                    
                }
                if (context.ProductsTypes != null && !context.ProductsTypes.Any())
                {
                    var typesData = File.ReadAllText("../Store.Repository/SeedData/types.json");

                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    if (types is not null)
                    
                        await context.ProductsTypes.AddRangeAsync(types);

                }
                if (context.Products != null && !context.Products.Any())
                {
                    var productsData = File.ReadAllText("../Store.Repository/SeedData/products.json");

                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    if (products is not null)
                    
                        await context.Products.AddRangeAsync(products); 
                }
                

            
               if (context.DeliveryMethods != null && !context.DeliveryMethods.Any())
            {
                var deliveryMethodsData = File.ReadAllText("../Store.Repository/SeedData/delivery.json");

                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);
                if (deliveryMethods is not null)

                    await context.DeliveryMethods.AddRangeAsync(deliveryMethods);
            }
            await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
