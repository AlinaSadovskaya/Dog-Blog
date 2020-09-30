using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
       // public TsUser TsUser { get; set; }
        public string Contents { get; set; }

        public string UserID { get; set; }
        public User User { get; set; }
    }
}
