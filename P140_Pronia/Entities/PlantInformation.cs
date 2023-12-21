namespace P140_Pronia.Entities
{
    public class PlantInformation
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public int InformationId { get; set; }
        public Plant Plant { get; set; } = null!;
        public Information Information { get; set; } = null!;
    }
}
