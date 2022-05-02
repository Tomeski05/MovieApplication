using MovieApp.Domain;
using MovieApp.Models;
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

        public async Task<int> CreateAsync(MovieVM movie)
        {
            return await _movieRepository.CreateAsync(movie);
        }

        public async Task<int> DeleteAsync(MovieVM movie)
        {
            return await _movieRepository.DeleteAsync(movie);
        }

        public async Task<List<Movies>> GetAll()
        {
            return await _movieRepository.GetAllAsync();
        }

        public async Task<Movies> GetById(int id)
        {
            return await _movieRepository.GetByIdAsync(id);
        }

        public async Task<int> UpdateAsync(MovieVM movie)
        {
            return await _movieRepository.UpdateAsync(movie);
        }
    }
}
