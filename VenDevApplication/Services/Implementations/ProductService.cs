using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VenDevApplication.Models;
using VenDevApplication.Services.Interfaces;

namespace VenDevApplication.Services.Implementations
{
    public class ProductService : Iproduct
    {
        private readonly ApplicationDbContext _context ;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddProduct(Product product)
        {
           try
           {
               await _context.products.AddAsync(product);
               await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteProductById(int productId)
        {
            try 
            {
                var product = await GetProductById(productId);
                 _context.products.Remove(product);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Product>> GetListProduct()
        {
            return await _context.products.ToListAsync();
        }

        public async Task<Product> GetProductById(int productId)
        {
            try 
            {
                var product = await _context.products.FindAsync(productId); 
                return product;
            } catch(Exception e)    
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            try
            {
                _context.products.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<string> GetProductImageName(int id)
        {
            var imagename = await _context.products.Where(p =>p.Id==id).Select(p =>p.Image).FirstOrDefaultAsync() ;
            return imagename ?? "";

        }
    }
}
