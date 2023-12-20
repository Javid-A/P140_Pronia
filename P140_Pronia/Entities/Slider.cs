namespace P140_Pronia.Entities
{
    public class Slider
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Offer { get; set; } = null!;
        public string ShortDesc { get; set; } = null!;
        public string URL { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string? BgImage { get; set; } = null!;
    }
}
