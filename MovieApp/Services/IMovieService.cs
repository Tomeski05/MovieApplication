using MovieApp.Domain;
using MovieApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Services
{
    public interface IMovieService
    {
        public Task<List<Movies>> GetAll();
        public Task<Movies> GetById(int id);
        public Task<int> CreateAsync(MovieVM movie);
        public Task<int> UpdateAsync(MovieVM movie);
        public Task<int> DeleteAsync(MovieVM movie);
    }
}
