using System;

namespace Core.Entities
{
    public class Person :BaseEntity
    {
       
        public  string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public  string PictureUrl { get; set; }
        
        public DateTimeOffset LastLogin { get; set; }
    }
}