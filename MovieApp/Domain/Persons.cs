using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Domain
{
    public class Persons
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public List<Roles> Role { get; set; }
    }
}
