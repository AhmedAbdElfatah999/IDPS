using System;
namespace API.Dtos
{
    public class MessageToReturnDto
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string PatientName { get; set; }
        public string PatientPhotoUrl { get; set; }
        public string RecipientId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorPhotoUrl { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }

        public string DoctorId { get; set; }
        public string PatientId { get; set; }
    }
}