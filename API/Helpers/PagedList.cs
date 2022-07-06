using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            //if we get a total cout of 10 items & pageSize of 5, then we've got 2 pages
            TotalPages = (int) Math.Ceiling(count / (double) pageSize); 
            PageSize = pageSize; //the number of users displayed per page
            TotalCount = count;
            AddRange(items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        //this is an instance of the Class PagedList that returns a PagedList by working out the count and items
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, 
            int pageSize)
        {
            var count = await source.CountAsync(); // get the total number of records from db
            var items = await source.Skip((pageNumber -1)* pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
        
    }
}