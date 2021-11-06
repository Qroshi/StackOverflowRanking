using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackOverflowRanking.Models
{
    public class Tags
    {
        public List<Tag> items { get; set; }

        public Tags()
        {
            this.items = new List<Tag>();
        }
    }
}
