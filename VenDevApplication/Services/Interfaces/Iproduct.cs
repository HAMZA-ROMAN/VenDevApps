using System.Collections.Generic;
using System.Threading.Tasks;
using VenDevApplication.Models;

namespace VenDevApplication.Services.Interfaces
{
    public interface Iproduct
    {
        public Task<bool> AddProduct(Product product);
        public Task<bool> UpdateProduct(Product product);
        public Task<Product> GetProductById(int productId);
        public Task<bool> DeleteProductById(int productId);
        public Task<IEnumerable<Product>> GetListProduct();
        public  Task<string> GetProductImageName(int id);


    }
}
