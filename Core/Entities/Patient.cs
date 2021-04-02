namespace Core.Entities
{
    public class Patient :Person
    {
        public double Weight { get; set; }
        public double Hight { get; set; }
        public string BloodType { get; set; }
    }
}