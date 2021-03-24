namespace Core.Entities
{
    public class Disease :BaseEntity
    {
       // public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
       public virtual Specialization Specialization {get; set;}
        public int SpecializationId { get; set; }
        
    }
}