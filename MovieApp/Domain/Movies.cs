using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Domain
{
    public class Movies: BaseEntity
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public DateTime YearReleased { get; set; }
        public Genres Genre { get; set; }
        public List<Persons> Persons { get; set; }
    }
}
