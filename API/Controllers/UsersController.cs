using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController( IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        //-The public methods of the controller are called action methods or simply actions. 
        //-When the Web API framework receives a request, it routes the request to an action.
        //-ActionResult<T> is a return type for web API controller actions
        //*IEnumerable in C# is an interface that defines one method, GetEnumerator-
        //-which returns an IEnumerator interface. This allows read-only access to a collection (lists in an array or from database)
        //-then a collection that implements IEnumerable can be used with a for-each statement.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            
            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
           
            return  await _userRepository.GetMemberAsync(username);
        }
    }
}