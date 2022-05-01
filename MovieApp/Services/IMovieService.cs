using MovieApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Services
{
    public interface IMovieService
    {
        public Task<List<Movies>> GetAllProducts();
        public Task<Movies> GetProductById(int id);
        public Task<int> CreateProductAsync(Movies product);
        public Task<int> UpdateProductAsync(Movies product);
        public Task<int> DeleteProductAsync(Movies product);
    }
}
