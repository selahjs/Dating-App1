using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, 
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, 
        IdentityRoleClaim<int>,IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        // you no longer need to create Users table becuase Identity will provide with the User table we need
        // public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages {get; set;}

        protected override void OnModelCreating(ModelBuilder builder){
            //we are trying to further configure our likes entity
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur=> ur.UserId)
                .IsRequired();
            
            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur=> ur.RoleId)
                .IsRequired();

            builder.Entity<UserLike>()
                .HasKey(k=> new {k.SourceUserId, k.LikedUserId}); //this table will have a primary key of combination source and liked user id
            //then we configure the many to many relationship 
            //a source user can have many liked users
            
            //a user can like many users
            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s=> s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade); //when we delete the user we also delete the related entity(the like of user he liked)
            
            //a user can be liked by many users
            builder.Entity<UserLike>()
                .HasOne(s => s.LikedUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(l=> l.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade); //when we delete the user we also delete the related entity(the like of user he liked)

            builder.Entity<Message>()
                .HasOne(u=> u.Recipient)
                .WithMany(m => m.MessagesRecieved)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Message>()
                .HasOne(u=> u.Sender)
                .WithMany(m => m.MessegesSent)
                .OnDelete(DeleteBehavior.Restrict);
                
        }

    }
}