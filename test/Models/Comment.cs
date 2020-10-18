﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace test.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual Post Post { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }
        public int Like { get; set; } = 0;
        public int Dislike { get; set; } = 0;

    }
}
