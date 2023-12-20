namespace P140_Pronia.Entities
{
    public class PlantImage
    {
        public int Id { get; set; }
        public string Path { get; set; } = null!;
        public bool? IsMain { get; set; }
        public int PlantId { get; set; }
        public Plant Plant { get; set; } = null!;
    }
}
