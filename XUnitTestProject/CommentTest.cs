using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using test.Domain.Core;
using test.Infrastructure.Data;
using test.Services.BusinessLogic;
using Xunit;

namespace test.Services.XUnitTestProject
{
    public class CommentTest
    {
         private CommentRepository repository;
         private PostRepository postRepository;
         public static DbContextOptions<BlogContext> dbContextOptions { get; }
         public static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=RazorPagesMovieContext15-bc;Trusted_Connection=True;MultipleActiveResultSets=true";

         static CommentTest()
         {
             dbContextOptions = new DbContextOptionsBuilder<BlogContext>()
                 .UseSqlServer(connectionString)
                 .Options;
         }

         public CommentTest()
         {
             var context = new BlogContext(dbContextOptions);
             InicializeDB db = new InicializeDB();
             db.Seed(context);
             repository = new CommentRepository(context);
             postRepository = new PostRepository(context);
        }

         [Fact]
         public async void TestCreate()
         {
             var Comments = repository.FindAllByPost(3);
             var Posts = await postRepository.FirstOrDefaultAsync(3);
             Assert.NotNull(Comments);
             Assert.Single(Comments);
             await repository.Create(new Comment { DateTime = DateTime.Now, Post = Posts, Text = "Test5" });
             Comments = repository.FindAllByPost(3);
             Assert.NotNull(Comments);
             Assert.Equal(2, Comments.Count);
         }
    }
}
