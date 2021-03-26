namespace Core.Entities
{
    public class Pharmacy:BaseEntity
    {
        
        public string Name { get; set; }
        public string Phone { get; set; }
        public string PictureUrl { get; set; }

        public int NumberOfBranches { get; set; }
    }
}