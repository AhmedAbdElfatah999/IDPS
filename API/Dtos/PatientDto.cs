using System;

namespace API.Dtos
{
    public class PatientDto
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public TimeSpan LastLogin { get; set; }
        public string Id { get; set; } 
        public string PhotoUrl { get; set; }       
    }
}