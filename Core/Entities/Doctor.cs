namespace Core.Entities
{
    public class Doctor :Person
    {
        public string MyClinicAddress { get; set; }
        public  string MyClinicName { get; set; }
        public int WorkHours { get; set; }

       public virtual Specialization Specialization {get; set;}
        public int SpecializationId { get; set; }

        public bool IsActivated=false;

    }
}