namespace Core.Entities
{
    public class Hospital:BaseEntity
    {
       
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PictureUrl { get; set; }
        public string Details { get; set; }
        
    }
}