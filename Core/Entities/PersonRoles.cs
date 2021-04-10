
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
   [NotMapped]
    public class PersonRoles :IdentityRole<string>
    {
       //public int Id { get; set; }
        public const string Admin = "Admin";  
        public const string Patient = "Patient";
        public const string Doctor = "Doctor";  
    }
}