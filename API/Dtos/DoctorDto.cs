using System;

namespace API.Dtos
{
    public class DoctorDto
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public TimeSpan LastLogin { get; set; }
    }
}