namespace Dashboard_Backend.Models
{
    public class SimplifiedSalesData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string AffiliateId { get; set; }
        public string CardId { get; set; }
    }

    //public class SalesData
    //{
    //    public string Id { get; set; }
    //    public string ProductId { get; set; }
    //    public ProductData Product { get; set; }
    //    public string AffiliateId { get; set; }
    //    public string CardId { get; set; }
    //}

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
