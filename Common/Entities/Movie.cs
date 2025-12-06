using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Movie : BaseEntity
    {
        public int DirectorId { get; set; }

        public string Title { get; set; }
        public string CoverPath { get; set; }
        public string Release_Date { get; set; }
        public double Rating { get; set; }

        public virtual Director Director { get; set; }
        public virtual List<Review> Reviews { get; set; }
        public virtual List<MovieActor> Actors { get; set; }
        public virtual List<MovieGenre> Genres { get; set; }
    }
}
