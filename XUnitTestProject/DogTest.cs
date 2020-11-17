using Microsoft.EntityFrameworkCore;
using System;
using test.Domain.Core;
using test.Infrastructure.Data;
using test.Services.BusinessLogic;
using test.Services.XUnitTestProject;
using Xunit;

namespace XUnitTestProject
{
    public class DogTest
    {
        private DogRepository repository;
        public static DbContextOptions<BlogContext> dbContextOptions { get; }
        public static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=RazorPagesMovieContext13-bc;Trusted_Connection=True;MultipleActiveResultSets=true";

        static DogTest()
        {

            dbContextOptions = new DbContextOptionsBuilder<BlogContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public DogTest()
        {
            var context = new BlogContext(dbContextOptions);
            InicializeDB db = new InicializeDB();
            db.Seed(context);
            repository = new DogRepository(context);
        }
        [Fact]
                                                                                                                                                                                                                                                                                                                public async void TestFindAllTag()
        {
            var Dogs = await repository.FindAll();
            Assert.NotNull(Dogs);
            Assert.Equal(2, Dogs.Count);
        }
        [Fact]
        public async void TestFirstOrDefaultAsyncDog()
        {
            var Dog = await repository.FirstOrDefaultAsync(2);
            Assert.NotNull(Dog);
            Assert.Equal("Test2", Dog.DogName);
            Assert.Equal("Test2", Dog.DogDescription);
        }

        [Fact]
        public async void TestRemoveDog()
        {
            var Dog = await repository.FirstOrDefaultAsync(2);
            await repository.Remove(Dog);
            var Dogs = await repository.FindAll();
            Assert.NotNull(Dogs);
            Assert.Single(Dogs);
        }


        [Fact]
        public async void TestCreateDog()
        {
            await repository.Create(new Dog { DogName = "Test3", DogDescription = "Test3" });
            var DogsLst = await repository.FindAll();
            Assert.NotNull(DogsLst);
            Assert.Equal(3, DogsLst.Count);
        }
        [Fact]
        public async void TestUpdateDog()
        {
            var Dog = await repository.FirstOrDefaultAsync(2);
            Dog.DogDescription = "Updated";
            await repository.Update(Dog);
            Dog = await repository.FirstOrDefaultAsync(2);
            Assert.NotNull(Dog);
            Assert.Equal("Updated", Dog.DogDescription);
        }

    }
}
