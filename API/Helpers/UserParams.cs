using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class UserParams : PaginationParams
    {
        public string CurrentUsername { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 150; //so that users get all users and decide for them selfs what they want to filter
        public string OrederBy { get; set; } = "lastActive"; //by default users will be sorted by last Active
    }

}