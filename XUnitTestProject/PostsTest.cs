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
    public class PostsTest
    {
        private PostRepository repository;
        public static DbContextOptions<BlogContext> dbContextOptions { get; }
        public static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=RazorPagesMovieContext14-bc;Trusted_Connection=True;MultipleActiveResultSets=true";

        static PostsTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<BlogContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public PostsTest()
        {
            var context = new BlogContext(dbContextOptions);
            InicializeDB db = new InicializeDB();
            db.Seed(context);
            repository = new PostRepository(context);
        }

        [Fact]
        public async void TestFindFirst()
        {
            var Post = await repository.FirstOrDefaultAsync(1);
            Assert.NotNull(Post);
            Assert.Equal("Test1", Post.Text);
            Post = await repository.FirstOrDefaultAsync(6);
            Assert.Null(Post);
        }
        [Fact]
        public async void TestRemove()
        {
            var Posts = await repository.FindAll();
            Assert.NotNull(Posts);
            Assert.Equal(4, Posts.Count);
            await repository.Remove(await repository.FirstOrDefaultAsync(2));
            Posts = await repository.FindAll();
            Assert.NotNull(Posts);
            Assert.Equal(3, Posts.Count);
        }
        [Fact]
        public async void TestCreate()
        {
            var Posts = await repository.FindAll();
            Assert.NotNull(Posts);
            Assert.Equal(4, Posts.Count);
            await repository.Create(new Post { DateTime = DateTime.Now, Title = "First", Text = "Test3", Topic = new Topic { TopicName = "Test6" } });
            Posts = await repository.FindAll();
            Assert.NotNull(Posts);
            Assert.Equal(5, Posts.Count);
        }
        [Fact]
        public async void TestUpdate()
        {
            var Posts = await repository.FirstOrDefaultAsync(2);
            Posts.Text = "Updated";
            await repository.Update(Posts);
            Posts = await repository.FirstOrDefaultAsync(2);
            Assert.NotNull(Posts);
            Assert.Equal("Updated", Posts.Text);
        }
    }
}
