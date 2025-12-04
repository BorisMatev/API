using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; }
        public string CoverPath { get; set; }
        public string Release_Date { get; set; }
        public double Rating { get; set; }
    }
}
