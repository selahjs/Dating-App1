using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        // Creating User property of type AppUser, DbSet represents a table in the database
        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages {get; set;}

        protected override void OnModelCreating(ModelBuilder builder){
            //we are trying to further configure our likes entity
            base.OnModelCreating(builder);

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