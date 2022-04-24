using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    //This Class is used to return A Memeber Object with a limmited imformation
    //i.e. excluding password - when a user loges in the api returns a member dto
    //The Automapper is used in this case to convert/Map the AppUser entity to a MemberDto
    public class MemberDto
    {
        public int Id { get; set; }
        public string Username { get; set; } //this naming is used inside the client app -- angular. 
                                            //-makes sense because only DTOs are returned to the client
                                            // what you see as a response in angular is exatly how the client
                                            // will get it
        public string PhotoUrl { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
    }
}