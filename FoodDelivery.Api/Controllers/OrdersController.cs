using FoodDelivery.Api.DTOs;
using FoodDelivery.Api.Interfaces;
using FoodDelivery.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace FoodDelivery.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    /// <summary>
    /// Retrieves all orders.
    /// </summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all orders", Description = "Returns all available orders in the system.")]
    [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetAllAsync(cancellationToken);
        return Ok(orders);
    }

    /// <summary>
    /// Retrieves an order by its unique identifier.
    /// </summary>
    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Get an order by id", Description = "Returns a single order based on the supplied id.")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest(new { message = "Order id must be greater than zero." });
        }

        var order = await _orderService.GetByIdAsync(id, cancellationToken);
        return Ok(order);
    }

    /// <summary>
    /// Searches orders using a keyword.
    /// </summary>
    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search orders", Description = "Searches orders by customer name, phone, food item, or delivery address.")]
    [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync([FromQuery] string? searchTerm, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest(new { message = "Search term is required." });
        }

        var orders = await _orderService.SearchAsync(searchTerm, cancellationToken);
        return Ok(orders);
    }

    /// <summary>
    /// Creates a new order.
    /// </summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Create an order", Description = "Creates a new order record in the internal system.")]
    [SwaggerRequestExample(typeof(Order), typeof(OrderRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(OrderResponseExample))]
    [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] Order order, CancellationToken cancellationToken)
    {
        if (order is null)
        {
            return BadRequest(new { message = "Order payload is required." });
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var createdOrder = await _orderService.CreateAsync(order, cancellationToken);
        return Created($"/api/orders/{createdOrder.Id}", createdOrder);
    }

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Update an order", Description = "Updates the full order payload for an existing order record.")]
    [SwaggerRequestExample(typeof(Order), typeof(OrderRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(OrderResponseExample))]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] Order order, CancellationToken cancellationToken)
    {
        if (order is null)
        {
            return BadRequest(new { message = "Order payload is required." });
        }

        if (id != order.Id)
        {
            return BadRequest(new { message = "Route id must match the order id in the payload." });
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var updatedOrder = await _orderService.UpdateAsync(order, cancellationToken);
        return Ok(updatedOrder);
    }

    /// <summary>
    /// Updates the status of an order.
    /// </summary>
    [HttpPatch("{id:int}/status")]
    [SwaggerOperation(Summary = "Update order status", Description = "Updates the current status of a specific order.")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatusAsync(int id, [FromBody] OrderStatus status, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest(new { message = "Order id must be greater than zero." });
        }

        var updatedOrder = await _orderService.UpdateStatusAsync(id, status, cancellationToken);
        return Ok(updatedOrder);
    }

    /// <summary>
    /// Deletes an order by its identifier.
    /// </summary>
    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Delete an order", Description = "Deletes an existing order record from the system.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest(new { message = "Order id must be greater than zero." });
        }

        var deleted = await _orderService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound(new { message = $"Order with id {id} was not found." });
    }

    /// <summary>
    /// Retrieves the dashboard summary for orders.
    /// </summary>
    [HttpGet("summary")]
    [SwaggerOperation(Summary = "Get dashboard summary", Description = "Returns the order summary metrics required for the internal dashboard.")]
    [ProducesResponseType(typeof(DashboardSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummaryAsync(CancellationToken cancellationToken)
    {
        var summary = await _orderService.GetSummaryAsync(cancellationToken);

        var dashboardSummary = new DashboardSummaryDto
        {
            TotalOrders = summary.TotalOrders,
            PlacedOrders = summary.PlacedCount,
            PreparingOrders = summary.PreparingCount,
            OutForDeliveryOrders = summary.OutForDeliveryCount,
            DeliveredOrders = summary.DeliveredCount,
            CancelledOrders = summary.CancelledCount,
            TotalRevenue = summary.TotalRevenue
        };

        return Ok(dashboardSummary);
    }
}
