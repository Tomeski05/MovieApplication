using MovieApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class MovieVM
    {
        public string Title { get; set; }
        public DateTime YearReleased { get; set; }
        public int PersonsId { get; set; }
        public int GenreId { get; set; }
    }
}
