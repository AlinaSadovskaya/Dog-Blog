using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace test.Models
{
    public class Post
    {
        public int PostId { get; set; }
        //public virtual TsUser User { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string HtmlContents { get; set; }
        public string RawHtmlContents { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
        public IEnumerable<Topic> Topics { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

    }
}
