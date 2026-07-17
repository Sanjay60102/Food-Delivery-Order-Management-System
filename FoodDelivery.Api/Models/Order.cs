using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Api.Models;

public enum OrderStatus
{
    Placed,
    Preparing,
    OutForDelivery,
    Delivered,
    Cancelled
}

public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(20)]
    public string CustomerPhone { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string FoodItem { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [StringLength(250)]
    public string DeliveryAddress { get; set; } = string.Empty;

    [Required]
    public OrderStatus Status { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }
}
