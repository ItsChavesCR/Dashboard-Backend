namespace Dashboard_Backend.Models
{
    public class SalesData
    { 
        public string Id { get; set; }
       public string ProductId { get; set; }
        public ProductData Product { get; set; }
       public string AffiliateId { get; set; }
      public string CardId { get; set; }
    }
}
