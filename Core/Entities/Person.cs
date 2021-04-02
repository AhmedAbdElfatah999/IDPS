using System;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class Person : IdentityUser
    {
       
        public  string Name { get; set; }
        public string Gender { get; set; }
        public  string PictureUrl { get; set; }
        public string Address {get; set;}
        public DateTimeOffset LastLogin { get; set; }
    }
}