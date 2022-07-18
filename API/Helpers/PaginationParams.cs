using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class PaginationParams
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
    }
}