using Microsoft.EntityFrameworkCore;
using RedSocial.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSocial.Infraestructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
          
        public DbSet<Post> post {  get; set; }
        public DbSet<Friend> friend { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Tables
            modelBuilder.Entity<Post>().ToTable("Post");
            modelBuilder.Entity<Friend>().ToTable("Friends");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            #endregion

            #region Keys
            modelBuilder.Entity<Post>().HasKey(p => p.Id);
            modelBuilder.Entity<Friend>().HasKey(f => f.Id);
            modelBuilder.Entity<Comment>().HasKey(c => c.Id);

            #endregion

            #region RelationShip
            modelBuilder.Entity<Comment>()
               .HasOne(c => c.Post)
               .WithMany(p => p.Comments)
               .HasForeignKey(c => c.PostID);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(cp => cp.Replies)
                .HasForeignKey(c => c.ParentCommentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostID);


            #region Property Configuration

            #region Post
            modelBuilder.Entity<Post>().
                Property(p => p.PublicationType)
                .IsRequired();

            modelBuilder.Entity<Post>().
               Property(p => p.PostDate)
               .IsRequired();
            #endregion

            #region Friend
            modelBuilder.Entity<Friend>().
                Property(f => f.User_Id1)
                .IsRequired();

            modelBuilder.Entity<Friend>().
               Property(f => f.User_Id2)
               .IsRequired();
            #endregion

            #region Comment
            modelBuilder.Entity<Comment>().
               Property(c => c.PostID)
               .IsRequired();

            modelBuilder.Entity<Comment>().
               Property(c => c.UserID)
               .IsRequired();
            #endregion
            #endregion
            #endregion
        }
    }
}
