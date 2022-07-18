using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class UserLike
    {
        public AppUser SourceUser { get; set; } //the user that is liking the outher user
        public int SourceUserId { get; set; }
        public AppUser LikedUser { get; set; } //the user that is getting likes
        public int LikedUserId { get; set; }
    }
}