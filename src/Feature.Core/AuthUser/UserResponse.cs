using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Core.AuthUser
{
    public class UserResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
