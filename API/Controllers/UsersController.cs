using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extentions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public UsersController( IUserRepository userRepository, IMapper mapper, 
            IPhotoService photoService)
        {
            _photoService = photoService;
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

        [HttpGet("{username}", Name = "GetUser")] //"username" is a root parameter
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
           
            return  await _userRepository.GetMemberAsync(username);
        }

        [HttpPut]
        //we are not returning anything from this method because the user has everything it needs
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            _mapper.Map(memberUpdateDto, user);
            _userRepository.Update(user);

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to Update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            //we get a single user becuase we are uploading a photo to a single user
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file);

            //if there is no error we get null
            if(result.Error != null) return BadRequest(result.Error.Message);

            //if photo is uploaded successfully.
            //..we are saving the url and id of the newly created photo to the database
            var photo = new Photo 
            {
                Url = result.SecureUrl.AbsoluteUri, //we are getting the url & publicId from Cloudinary.com
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            //saving it to the database
            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync())
            {
                // we are returning the route of how to  get the user and our photo object
                return CreatedAtRoute("GetUser",new {username = user.UserName}, _mapper.Map<PhotoDto>(photo));
            }                
            return BadRequest("Problem adding Photo");

        }
        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId){
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId); //get the photo where the id matches

            if (photo.IsMain) return BadRequest("This is already your main photo");
            //if user is setting the Main photo main again

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);//getting the current main photo
            if(currentMain != null) currentMain.IsMain = false; // we are setting the current photo to not main
            photo.IsMain = true; //setting the newly created photo to main

            if (await _userRepository.SaveAllAsync()) return NoContent();//if successful
            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId){
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("You can not delete your main photo");

            if(photo.PublicId != null){
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo");
        }

    }
}