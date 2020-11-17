using System;
using System.Collections.Generic;
using System.Text;
using test.Domain.Core;
using test.Infrastructure.Data;

namespace test.Services.XUnitTestProject
{
    class InicializeDB
    {
        public InicializeDB()
        {
        }

        public void Seed(BlogContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Topics.AddRange(
                new Topic { TopicName = "Test1" },
                new Topic { TopicName = "Test2" },
                new Topic { TopicName = "Test3" }
                );
            context.Dogs.AddRange(
                new Dog { DogName = "Test1", DogDescription = "Test1"},
                new Dog { DogName = "Test2", DogDescription = "Test2" }
                );
            context.Posts.AddRange(
                new Post { DateTime = DateTime.Now, Title = "test1", Text = "Test1", Topic = new Topic { TopicName = "Test4" } },
                new Post { DateTime = DateTime.Now, Title = "test2", Text = "Test2", Topic = new Topic { TopicName = "Test5" } }
                );
            context.Comments.AddRange(
                new Comment { DateTime = DateTime.Now, Post = new Post { DateTime = DateTime.Now, Title = "test3", Text = "Test3", Topic = new Topic { TopicName = "Test6" } }, Text = "Test1" },
                new Comment { DateTime = DateTime.Now, Post = new Post { DateTime = DateTime.Now, Title = "test2", Text = "Test4", Topic = new Topic { TopicName = "Test7" } }, Text = "Test2" }
                );
            context.SaveChanges();
        }
    }
}
