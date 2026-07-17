using FoodDelivery.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace FoodDelivery.Api.DTOs;

public class OrderRequestExample : IExamplesProvider<Order>
{
    public Order GetExamples()
    {
        return new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            CustomerPhone = "9876543210",
            FoodItem = "Paneer Roll",
            Quantity = 2,
            Price = 180.00m,
            DeliveryAddress = "12 Market Road, Hyderabad",
            Status = OrderStatus.Placed,
            OrderDate = DateTime.UtcNow
        };
    }
}

public class OrderResponseExample : IExamplesProvider<Order>
{
    public Order GetExamples()
    {
        return new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            CustomerPhone = "9876543210",
            FoodItem = "Paneer Roll",
            Quantity = 2,
            Price = 180.00m,
            DeliveryAddress = "12 Market Road, Hyderabad",
            Status = OrderStatus.Placed,
            OrderDate = DateTime.UtcNow
        };
    }
}
