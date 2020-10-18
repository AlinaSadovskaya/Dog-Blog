using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace test.Models
{
    public class Post
    {
        [HiddenInput(DisplayValue = false)]
        public int PostId { get; set; }
        [Required(ErrorMessage = "Введите название статьи!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Введите статью!")]
        public string Text { get; set; }

        public string Image { get; set; }

        public int TopicId { get; set; }
        public Topic Topic { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Comment> Comments { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }

        public int Like { get; set; } = 0;
        public int Dislike { get; set; } = 0;

        public Post()
        {
            Comments = new List<Comment>();
        }

    }
}
