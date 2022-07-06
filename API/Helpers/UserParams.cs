using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class UserParams
    {
        //this is a pagination parameter we get from our users to feed our GetUsers method/endpoint
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;//the default value is 1
        private int _pageSize = 10; //this is the default size of items desplayde inside one page
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            //if the value is greate than MaxPageSize reset it ti 50 else set it to the user input 
        }
        public string CurrentUsername { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 150; //so that users get all users and decide for them selfs what they want to filter
        public string OrederBy { get; set; } = "lastActive"; //by default users will be sorted by last Active
    }

}