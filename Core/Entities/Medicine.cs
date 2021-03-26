namespace Core.Entities
{
    public class Medicine:BaseEntity
    {

        public string Name  { get; set; }
        public string Manufacturer { get; set; }

        public double Price { get; set; }
        public string HowToTake { get; set; }
        public  string PictureUrl { get; set; }
        public string Description { get; set; }
    }
}