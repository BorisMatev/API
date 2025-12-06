using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Director : BaseEntity
    {
        public string Name { get; set; }
        public string Biography { get; set; }
        public virtual List<Movie> Movies { get; set; }
    }
}
