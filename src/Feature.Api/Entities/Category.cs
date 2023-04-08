using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Api.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
