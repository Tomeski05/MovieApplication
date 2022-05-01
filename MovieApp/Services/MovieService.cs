using MovieApp.Domain;
using MovieApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<int> CreateProductAsync(Movies product)
        {
            return await _movieRepository.CreateAsync(product);
        }

        public async Task<int> DeleteProductAsync(Movies product)
        {
            return await _movieRepository.DeleteAsync(product);
        }

        public async Task<List<Movies>> GetAllProducts()
        {
            return await _movieRepository.GetAllAsync();
        }

        public async Task<Movies> GetProductById(int id)
        {
            return await _movieRepository.GetByIdAsync(id);
        }

        public async Task<int> UpdateProductAsync(Movies product)
        {
            return await _movieRepository.UpdateAsync(product);
        }
    }
}
