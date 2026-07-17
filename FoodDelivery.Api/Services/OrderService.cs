using FoodDelivery.Api.Exceptions;
using FoodDelivery.Api.Interfaces;
using FoodDelivery.Api.Models;

namespace FoodDelivery.Api.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _orderRepository.GetAllAsync(cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new ValidationException("Order id must be greater than zero.");
        }

        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);

        if (order is null)
        {
            throw new NotFoundException($"Order with id {id} was not found.");
        }

        return order;
    }

    public async Task<IEnumerable<Order>> SearchAsync(string? searchTerm, CancellationToken cancellationToken = default)
    {
        return await _orderRepository.SearchAsync(searchTerm, cancellationToken);
    }

    public async Task<Order> CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);
        ValidateOrder(order);

        if (order.OrderDate == default)
        {
            order.OrderDate = DateTime.UtcNow;
        }

        return await _orderRepository.CreateAsync(order, cancellationToken);
    }

    public async Task<Order?> UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (order.Id <= 0)
        {
            throw new ValidationException("Order id must be greater than zero.");
        }

        ValidateOrder(order);

        var updatedOrder = await _orderRepository.UpdateAsync(order, cancellationToken);

        if (updatedOrder is null)
        {
            throw new NotFoundException($"Order with id {order.Id} was not found.");
        }

        return updatedOrder;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new ValidationException("Order id must be greater than zero.");
        }

        var deleted = await _orderRepository.DeleteAsync(id, cancellationToken);

        if (!deleted)
        {
            throw new NotFoundException($"Order with id {id} was not found.");
        }

        return true;
    }

    public async Task<Order?> UpdateStatusAsync(int id, OrderStatus status, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new ValidationException("Order id must be greater than zero.");
        }

        if (!Enum.IsDefined(typeof(OrderStatus), status))
        {
            throw new ValidationException("Status must be a valid order status.");
        }

        var updatedOrder = await _orderRepository.UpdateStatusAsync(id, status, cancellationToken);

        if (updatedOrder is null)
        {
            throw new NotFoundException($"Order with id {id} was not found.");
        }

        return updatedOrder;
    }

    public async Task<OrderSummary> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        return await _orderRepository.GetSummaryAsync(cancellationToken);
    }

    private static void ValidateOrder(Order order)
    {
        if (string.IsNullOrWhiteSpace(order.CustomerName))
        {
            throw new ValidationException("Customer name is mandatory.");
        }

        if (string.IsNullOrWhiteSpace(order.CustomerPhone))
        {
            throw new ValidationException("Phone is mandatory.");
        }

        if (string.IsNullOrWhiteSpace(order.FoodItem))
        {
            throw new ValidationException("Food item is mandatory.");
        }

        if (string.IsNullOrWhiteSpace(order.DeliveryAddress))
        {
            throw new ValidationException("Delivery address is mandatory.");
        }

        if (order.Quantity <= 0)
        {
            throw new ValidationException("Quantity must be greater than zero.");
        }

        if (order.Price <= 0)
        {
            throw new ValidationException("Price must be greater than zero.");
        }

        if (!Enum.IsDefined(typeof(OrderStatus), order.Status))
        {
            throw new ValidationException("Status must be one of: Placed, Preparing, OutForDelivery, Delivered, Cancelled.");
        }
    }
}
