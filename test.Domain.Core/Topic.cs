using System;
using System.Collections.Generic;
using System.Text;

namespace test.Domain.Core
{
    public class Topic
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
