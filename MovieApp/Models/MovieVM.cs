using MovieApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class MovieVM
    {
        public Movies Movie { get; set; }
        public Persons Person { get; set; }
        public Genres Genre { get; set; }
        public Roles Role { get; set; }
    }
}
