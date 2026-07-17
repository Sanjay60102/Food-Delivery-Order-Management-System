using FoodDelivery.Api.Models;

namespace FoodDelivery.Api.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> SearchAsync(string? searchTerm, CancellationToken cancellationToken = default);
    Task<Order> CreateAsync(Order order, CancellationToken cancellationToken = default);
    Task<Order?> UpdateAsync(Order order, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Order?> UpdateStatusAsync(int id, OrderStatus status, CancellationToken cancellationToken = default);
    Task<OrderSummary> GetSummaryAsync(CancellationToken cancellationToken = default);
}
