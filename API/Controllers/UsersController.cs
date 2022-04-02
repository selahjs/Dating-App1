using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
        public UsersController( DataContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        //*In ASP.NET Web API, a controller is a class that handles HTTP requests. 
        //-The public methods of the controller are called action methods or simply actions. 
        //-When the Web API framework receives a request, it routes the request to an action.
        //-ActionResult<T> is a return type for web API controller actions
        //*Tasks in C# basically used to make your application more responsive and is basically used to- 
        //-implement Asynchronous Programming i.e. executing operations asynchronously
        //*IEnumerable in C# is an interface that defines one method, GetEnumerator-
        //-which returns an IEnumerator interface. This allows read-only access to a collection (lists in an array or from database)
        //-then a collection that implements IEnumerable can be used with a for-each statement.
        {
            return await _context.Users.ToListAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}