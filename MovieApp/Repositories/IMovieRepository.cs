using MovieApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Repositories
{
    public interface IMovieRepository: IRepository<Movies>
    {
    }
}
