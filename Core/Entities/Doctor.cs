using System;
using System.Collections;
using System.Collections.Generic;
namespace Core.Entities
{
    public class Doctor :Person
    {
        public string MyClinicAddress { get; set; }
        public  string MyClinicName { get; set; }
        public int WorkHours { get; set; }
        public virtual Specialization Specialization {get; set;}
        public int SpecializationId { get; set; }
        public int MyRate { get; set; }
        public bool IsActivated=false;

        public ICollection<Message> MyMessage { get; set; }
        public ICollection<Photo> Photos { get; set; }
        

    }
}