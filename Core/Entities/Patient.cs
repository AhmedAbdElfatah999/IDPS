using System;
using System.Collections;
using System.Collections.Generic;
namespace Core.Entities
{
    public class Patient :Person
    {
        public double Weight { get; set; }
        public double Hight { get; set; }
        public string BloodType { get; set; }

        public ICollection<Message> MyMessage { get; set; }
       public ICollection<Photo> Photos { get; set; }
 
    }
}