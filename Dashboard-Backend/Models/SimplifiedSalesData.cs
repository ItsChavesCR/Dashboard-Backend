namespace Dashboard_Backend.Models
{
    public class SimplifiedSalesData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string AffiliateId { get; set; }
        public string CardId { get; set; }
        public DateTime PurchaseDate { get; set; } // Nueva propiedad
        public decimal Amount { get; set; } // Nueva propiedad
    }

    public class ProductData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int Stock { get; set; }
        public string AffiliateId { get; set; }
    }
}
