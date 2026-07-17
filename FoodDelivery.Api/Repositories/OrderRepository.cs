using FoodDelivery.Api.Data;
using FoodDelivery.Api.Interfaces;
using FoodDelivery.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Api.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly FoodDeliveryDbContext _context;

    public OrderRepository(FoodDeliveryDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .AsNoTracking()
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            return null;
        }

        return await _context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Order>> SearchAsync(string? searchTerm, CancellationToken cancellationToken = default)
    {
        var term = searchTerm?.Trim();

        if (string.IsNullOrWhiteSpace(term))
        {
            return await GetAllAsync(cancellationToken);
        }

        return await _context.Orders
            .AsNoTracking()
            .Where(o => o.CustomerName.Contains(term)
                || o.CustomerPhone.Contains(term)
                || o.FoodItem.Contains(term)
                || o.DeliveryAddress.Contains(term))
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order> CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        return order;
    }

    public async Task<Order?> UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);

        var existingOrder = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken);

        if (existingOrder is null)
        {
            return null;
        }

        existingOrder.CustomerName = order.CustomerName;
        existingOrder.CustomerPhone = order.CustomerPhone;
        existingOrder.FoodItem = order.FoodItem;
        existingOrder.Quantity = order.Quantity;
        existingOrder.Price = order.Price;
        existingOrder.DeliveryAddress = order.DeliveryAddress;
        existingOrder.Status = order.Status;
        existingOrder.OrderDate = order.OrderDate;

        await _context.SaveChangesAsync(cancellationToken);

        return existingOrder;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            return false;
        }

        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (order is null)
        {
            return false;
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<Order?> UpdateStatusAsync(int id, OrderStatus status, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            return null;
        }

        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (order is null)
        {
            return null;
        }

        order.Status = status;
        await _context.SaveChangesAsync(cancellationToken);

        return order;
    }

    public async Task<OrderSummary> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        var summary = await _context.Orders
            .AsNoTracking()
            .GroupBy(_ => 1)
            .Select(g => new OrderSummary
            {
                TotalOrders = g.Count(),
                PlacedCount = g.Count(o => o.Status == OrderStatus.Placed),
                PreparingCount = g.Count(o => o.Status == OrderStatus.Preparing),
                OutForDeliveryCount = g.Count(o => o.Status == OrderStatus.OutForDelivery),
                DeliveredCount = g.Count(o => o.Status == OrderStatus.Delivered),
                CancelledCount = g.Count(o => o.Status == OrderStatus.Cancelled),
                TotalRevenue = g.Sum(o => o.Price)
            })
            .FirstOrDefaultAsync(cancellationToken) ?? new OrderSummary();

        return summary;
    }
}
