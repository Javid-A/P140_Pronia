namespace P140_Pronia.ViewModels
{
    public class CookieItem
    {
        public int Id { get; set; }
        public string ImagePath { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal UnitPrice { get; set; }
        public int  Quantity{ get; set; }
    }
}
