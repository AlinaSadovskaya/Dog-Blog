using test.Domain.Core;
using System.Collections.Generic;

namespace test.ViewModels
{
    public class PostIndexViewModel
    {
        public IEnumerable<Topic> Topics { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
