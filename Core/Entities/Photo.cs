namespace Core.Entities
{
    public class Photo
    {
         public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public Person User { get; set; }
        public string UserId { get; set; }       
    }
}