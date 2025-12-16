using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [NotMapped]
        public double Rating =>
        Reviews != null && Reviews.Count > 0
            ? Math.Round(Reviews.Average(r => r.Rating), 1)
            : 0;

        public virtual Director Director { get; set; }
        public virtual List<Review> Reviews { get; set; }
        public virtual List<MovieActor> Actors { get; set; }
        public virtual List<MovieGenre> Genres { get; set; }
    }
}
