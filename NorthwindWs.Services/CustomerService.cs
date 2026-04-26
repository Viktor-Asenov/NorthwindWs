using Microsoft.EntityFrameworkCore;
using NorthwindWs.Data;
using NorthwindWs.Services.Interfaces;
using NorthwindWs.Services.Models;

namespace NorthwindWs.Services;

public class CustomerService : ICustomerService
{
    private readonly NorthwindDbContext _context;

    public CustomerService(NorthwindDbContext context)
    {
        _context = context;
    }

    public async Task<List<CustomerOverviewDto>> GetCustomersOverviewAsync()
    {
        return await _context.Customers
            .Select(c => new CustomerOverviewDto
            {
                CustomerId = c.CustomerId,
                CompanyName = c.CompanyName,
                TotalOrders = c.Orders.Count
            })
            .ToListAsync();
    }

    public async Task<CustomerDetailDto?> GetCustomerDetailAsync(string customerId)
    {
        var customer = await _context.Customers
            .Include(c => c.Orders)
                .ThenInclude(o => o.OrderDetails)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (customer == null)
        {
            return null;
        }

        return new CustomerDetailDto
        {
            CustomerId = customer.CustomerId,
            CompanyName = customer.CompanyName,
            ContactName = customer.ContactName,
            City = customer.City,
            Country = customer.Country,
            Orders = customer.Orders.Select(o => new OrderSummaryDto
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                TotalValue = CalculateOrderTotal(o),
                ProductCount = o.OrderDetails.Count
            }).ToList()
        };
    }

    private decimal CalculateOrderTotal(Data.Entities.Order order)
    {
        return order.OrderDetails.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
    }
}
