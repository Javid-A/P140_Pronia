namespace P140_Pronia.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<PlantCategory> PlantCategories { get; set; }
    }
}
