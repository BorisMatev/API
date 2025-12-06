using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Actor : BaseEntity
    {
        public string Name { get; set; }
        public string Photo { get; set; }

        public virtual List<MovieActor> Movies { get; set; } 
    }
}
