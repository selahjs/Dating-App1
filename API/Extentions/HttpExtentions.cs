using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.Helpers;
using Microsoft.AspNetCore.Http;

namespace API.Extentions
{
    public static class HttpExtentions
    {
        public static void AddPaginationHeader(this HttpResponse response, 
            int currentPage, int itemsPerPage, int totalItems, int totalPages)
            //we just use this class to add Pagination headers inside our response
        {
            var paginationHeader = new PaginationHeader(currentPage,itemsPerPage,totalItems, totalPages);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
            //because we are adding a custom header, we need to add a CORS header on to paginationheader to make it available
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
            //we added two headers in this response

        }
    }
}