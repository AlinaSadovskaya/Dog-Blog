//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.Domain.Core;

namespace test.Infrastructure.Data
{
    public class BlogContext : IdentityDbContext<User>
    {

        public BlogContext(DbContextOptions<BlogContext> options)
            : base(options)
        {
            
        }
        public override DbSet<test.Domain.Core.User> Users { get; set; }
        public DbSet<Dog> Dogs { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
