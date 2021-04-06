namespace Core.Entities
{
    public class Pharmacy:BaseEntity
    {
        
        public string Name { get; set; }
        public string Phone { get; set; }
        public string PictureUrl { get; set; }

        public int NumberOfBranches { get; set; }
        public string Email { get; set; }
        public  string FacebookUrl { get; set; }
        public string  WebsiteUrl { get; set; }
    }
}