using ProductApi.Data;
using ProductApi.Models;

namespace ProductApi.BusinessLogic
{
    public class ProductConverter: IConverter
    {
        private readonly IRepository<Product> repository;
        public Product ProductDtoToProduct (ProductDto productDto) 
        {
            Product product = repository.Get(productDto.Id);
            if(product == null) 
            {
                throw new NullPointerException();   
            }
            return product;
        }
        public ProductDto ProductToProductDTO (Product product)
        {
            ProductDto productDto = new();
            productDto.Id = product.Id;
            productDto.Name = product.Name;
            productDto.Price = product.Price;
            productDto.ItemsInStock = product.ItemsInStock;
            productDto.ItemsReserved = product.ItemsReserved;
            return productDto;
        }
    }
}
