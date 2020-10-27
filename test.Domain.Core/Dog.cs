using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace test.Domain.Core
{
    public class Dog
    {
        public int DogId { get; set; }
        public string DogName { get; set; }
        public string DogDescription { get; set; }

        public string DogImage { get; set; }
    }
}
