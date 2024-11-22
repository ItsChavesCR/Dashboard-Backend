namespace Dashboard_Backend.Models
{
    public class SalesData
    {
        public string Id { get; set; }
        public List<ProductData> Products { get; set; } // Relación con múltiples productos
        public string AffiliateId { get; set; }
        public DateTime PurchaseDate { get; set; } // Fecha de compra
        public decimal Amount { get; set; } // Monto total de la compra
        public int Status { get; set; } // Estado de la compra


    }
}
