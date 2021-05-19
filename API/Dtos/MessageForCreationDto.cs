using System;
namespace API.Dtos
{
    public class MessageForCreationDto
    {
         public string DoctorId { get; set; }
        public string PatientId { get; set; }
         public string SenderId { get; set; }
        public string ReceieverId { get; set; }        
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }
        public MessageForCreationDto()
        {
            MessageSent = DateTime.Now;
        }
    }
}