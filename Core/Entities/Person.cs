using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class Person : IdentityUser
    {
       
        public  string Name { get; set; }
        public string Gender { get; set; }
        public  string PictureUrl { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$", ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 6 characters")]
        public string Password { get; set; }
        public string Address {get; set;}
        public DateTimeOffset LastLogin { get; set; }
    }
}