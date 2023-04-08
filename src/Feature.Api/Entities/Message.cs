using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Api.Entities
{
    public class Message
    {
        public Player Player { get; set; }
        public string Text { get; set; }
        public DateTime Date => DateTime.Now;
    }
}
