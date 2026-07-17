namespace FoodDelivery.Api.DTOs;

public class DashboardSummaryDto
{
    public int TotalOrders { get; set; }
    public int PlacedOrders { get; set; }
    public int PreparingOrders { get; set; }
    public int OutForDeliveryOrders { get; set; }
    public int DeliveredOrders { get; set; }
    public int CancelledOrders { get; set; }
    public decimal TotalRevenue { get; set; }
}
