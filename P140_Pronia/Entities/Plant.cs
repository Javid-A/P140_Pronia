namespace P140_Pronia.Entities
{
    public class Plant
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ICollection<PlantImage> PlantImages { get; set; } = null!;
        public ICollection<PlantCategory> PlantCategories { get; set; } = null!;
        public ICollection<PlantInformation> PlantInformations { get; set; } = null!;

    }
}
