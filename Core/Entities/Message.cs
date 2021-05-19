using System;
namespace Core.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        
        public string PatientId { get; set; }
        public Patient Patient { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
        public bool DoctorDeleted { get; set; }
        public bool PatientDeleted { get; set; }
        public string SenderId { get; set; }
        public string ReceieverId { get; set; }

    }
}