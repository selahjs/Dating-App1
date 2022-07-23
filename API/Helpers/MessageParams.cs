using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class MessageParams : PaginationParams
    {
        public string Username { get; set; } // a username is needed when gettin a user a message
        public string Container { get; set; } = "Unread";
        
    }
}