using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.Models
{
    public class Topic
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
