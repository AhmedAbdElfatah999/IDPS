namespace Core.Entities
{
    public class Chat:BaseEntity
    {
        
        public string Text { get; set; }

        public  Doctor  Doctor = new Doctor(); 

        public Patient Patient=new Patient();
    }
}