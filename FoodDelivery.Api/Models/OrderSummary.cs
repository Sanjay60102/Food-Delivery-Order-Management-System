namespace FoodDelivery.Api.Models;

public class OrderSummary
{
    public int TotalOrders { get; set; }
    public int PlacedCount { get; set; }
    public int PreparingCount { get; set; }
    public int OutForDeliveryCount { get; set; }
    public int DeliveredCount { get; set; }
    public int CancelledCount { get; set; }
    public decimal TotalRevenue { get; set; }
}
