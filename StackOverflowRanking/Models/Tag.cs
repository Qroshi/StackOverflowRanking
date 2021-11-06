using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackOverflowRanking.Models
{
    public class Tag
    {
        public double count { get; set; }
        public string name { get; set; }
        public double percent { get; set; }

        public Tag (double count, string name)
        {
            this.count = count;
            this.name = name;
        }
    }
}
