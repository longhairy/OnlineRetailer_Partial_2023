using ProductApi.Models;

namespace ProductApi.BusinessLogic
{
    public interface IConverter
    {
        public Product ProductDtoToProduct(ProductDto productDto);
        public ProductDto ProductToProductDTO(Product product);
    }
}
