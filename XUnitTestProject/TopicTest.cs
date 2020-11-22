using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using test.Infrastructure.Data;
using test.Services.BusinessLogic;
using Xunit;

namespace test.Services.XUnitTestProject
{
    public class TopicTest
    {
        private TopicRepository repository;
        public static DbContextOptions<BlogContext> dbContextOptions { get; }
        public static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=RazorPagesMovieContext12-bc;Trusted_Connection=True;MultipleActiveResultSets=true";

        static TopicTest()
        {

            dbContextOptions = new DbContextOptionsBuilder<BlogContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public TopicTest()
        {
            var context = new BlogContext(dbContextOptions);
            InicializeDB db = new InicializeDB();
            db.Seed(context);
            repository = new TopicRepository(context);
        }
        [Fact]
        public async void TestFindAllTag()
        {
            var Topic = await repository.FindAll();
            Assert.NotNull(Topic);
            Assert.Equal(7, Topic.Count);
        }
        [Fact]
        public async void TestFirstOrDefaultAsyncTopic()
        {
            var Topic = await repository.getSet().FirstOrDefaultAsync(m => m.TopicId == 3);
            Assert.NotNull(Topic);
            Assert.Equal("Test3", Topic.TopicName);
        }
        [Fact]
        public async void TestRemoveTopic()
        {
            var Topic = await repository.getSet().FirstOrDefaultAsync(m => m.TopicId == 3);
            await repository.Remove(Topic);
            var Topics = await repository.FindAll();
            Assert.NotNull(Topics);
            Assert.Equal(6, Topics.Count);
        }



    }
}
