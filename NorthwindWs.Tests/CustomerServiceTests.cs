using Microsoft.EntityFrameworkCore;
using NorthwindWs.Data;
using NorthwindWs.Data.Entities;
using NorthwindWs.Services;
using Xunit;

namespace NorthwindWs.Tests;

public class CustomerServiceTests : IDisposable
{
    private readonly NorthwindDbContext _context;
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        var options = new DbContextOptionsBuilder<NorthwindDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new NorthwindDbContext(options);
        _service = new CustomerService(_context);

        SeedTestData();
    }

    private void SeedTestData()
    {
        var customer1 = new Customer
        {
            CustomerId = "ALFKI",
            CompanyName = "Alfreds Futterkiste",
            ContactName = "Maria Anders",
            City = "Berlin",
            Country = "Germany"
        };

        var customer2 = new Customer
        {
            CustomerId = "ANATR",
            CompanyName = "Ana Trujillo Emparedados y helados",
            ContactName = "Ana Trujillo",
            City = "México D.F.",
            Country = "Mexico"
        };

        var customer3 = new Customer
        {
            CustomerId = "ANTON",
            CompanyName = "Antonio Moreno Taquería",
            ContactName = "Antonio Moreno",
            City = "México D.F.",
            Country = "Mexico"
        };

        var product1 = new Product
        {
            ProductId = 1,
            ProductName = "Chai",
            UnitPrice = 18.00m
        };

        var product2 = new Product
        {
            ProductId = 2,
            ProductName = "Chang",
            UnitPrice = 19.00m
        };

        var order1 = new Order
        {
            OrderId = 10248,
            CustomerId = "ALFKI",
            OrderDate = new DateTime(2024, 1, 1),
            Customer = customer1
        };

        var order2 = new Order
        {
            OrderId = 10249,
            CustomerId = "ALFKI",
            OrderDate = new DateTime(2024, 1, 2),
            Customer = customer1
        };

        var order3 = new Order
        {
            OrderId = 10250,
            CustomerId = "ANATR",
            OrderDate = new DateTime(2024, 1, 3),
            Customer = customer2
        };

        var orderDetail1 = new OrderDetail
        {
            OrderId = 10248,
            ProductId = 1,
            UnitPrice = 18.00m,
            Quantity = 10,
            Discount = 0.0f,
            Order = order1,
            Product = product1
        };

        var orderDetail2 = new OrderDetail
        {
            OrderId = 10248,
            ProductId = 2,
            UnitPrice = 19.00m,
            Quantity = 5,
            Discount = 0.1f,
            Order = order1,
            Product = product2
        };

        var orderDetail3 = new OrderDetail
        {
            OrderId = 10249,
            ProductId = 1,
            UnitPrice = 18.00m,
            Quantity = 15,
            Discount = 0.0f,
            Order = order2,
            Product = product1
        };

        var orderDetail4 = new OrderDetail
        {
            OrderId = 10250,
            ProductId = 2,
            UnitPrice = 19.00m,
            Quantity = 20,
            Discount = 0.05f,
            Order = order3,
            Product = product2
        };

        _context.Customers.AddRange(customer1, customer2, customer3);
        _context.Products.AddRange(product1, product2);
        _context.Orders.AddRange(order1, order2, order3);
        _context.OrderDetails.AddRange(orderDetail1, orderDetail2, orderDetail3, orderDetail4);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetCustomersOverviewAsync_ReturnsAllCustomers()
    {
        var result = await _service.GetCustomersOverviewAsync();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetCustomersOverviewAsync_ReturnsCorrectOrderCounts()
    {
        var result = await _service.GetCustomersOverviewAsync();

        var alfki = result.FirstOrDefault(c => c.CustomerId == "ALFKI");
        var anatr = result.FirstOrDefault(c => c.CustomerId == "ANATR");
        var anton = result.FirstOrDefault(c => c.CustomerId == "ANTON");

        Assert.NotNull(alfki);
        Assert.Equal(2, alfki.TotalOrders);

        Assert.NotNull(anatr);
        Assert.Equal(1, anatr.TotalOrders);

        Assert.NotNull(anton);
        Assert.Equal(0, anton.TotalOrders);
    }

    [Fact]
    public async Task GetCustomersOverviewAsync_ReturnsCorrectCompanyNames()
    {
        var result = await _service.GetCustomersOverviewAsync();

        var alfki = result.FirstOrDefault(c => c.CustomerId == "ALFKI");

        Assert.NotNull(alfki);
        Assert.Equal("Alfreds Futterkiste", alfki.CompanyName);
    }

    [Fact]
    public async Task GetCustomerDetailAsync_WithValidId_ReturnsCustomerDetail()
    {
        var result = await _service.GetCustomerDetailAsync("ALFKI");

        Assert.NotNull(result);
        Assert.Equal("ALFKI", result.CustomerId);
        Assert.Equal("Alfreds Futterkiste", result.CompanyName);
        Assert.Equal("Maria Anders", result.ContactName);
        Assert.Equal("Berlin", result.City);
        Assert.Equal("Germany", result.Country);
    }

    [Fact]
    public async Task GetCustomerDetailAsync_WithInvalidId_ReturnsNull()
    {
        var result = await _service.GetCustomerDetailAsync("INVALID");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetCustomerDetailAsync_ReturnsCorrectNumberOfOrders()
    {
        var result = await _service.GetCustomerDetailAsync("ALFKI");

        Assert.NotNull(result);
        Assert.Equal(2, result.Orders.Count);
    }

    [Fact]
    public async Task GetCustomerDetailAsync_CalculatesOrderTotalCorrectly()
    {
        var result = await _service.GetCustomerDetailAsync("ALFKI");

        Assert.NotNull(result);
        var order1 = result.Orders.FirstOrDefault(o => o.OrderId == 10248);

        Assert.NotNull(order1);
        // Order 10248: (18 * 10 * 1.0) + (19 * 5 * 0.9) = 180 + 85.5 = 265.5
        Assert.Equal(265.5m, order1.TotalValue);
    }

    [Fact]
    public async Task GetCustomerDetailAsync_CalculatesOrderTotalWithNoDiscount()
    {
        var result = await _service.GetCustomerDetailAsync("ALFKI");

        Assert.NotNull(result);
        var order2 = result.Orders.FirstOrDefault(o => o.OrderId == 10249);

        Assert.NotNull(order2);
        // Order 10249: (18 * 15 * 1.0) = 270
        Assert.Equal(270m, order2.TotalValue);
    }

    [Fact]
    public async Task GetCustomerDetailAsync_CalculatesOrderTotalWithDiscount()
    {
        var result = await _service.GetCustomerDetailAsync("ANATR");

        Assert.NotNull(result);
        var order = result.Orders.FirstOrDefault(o => o.OrderId == 10250);

        Assert.NotNull(order);
        // Order 10250: (19 * 20 * 0.95) = 361
        Assert.Equal(361m, order.TotalValue);
    }

    [Fact]
    public async Task GetCustomerDetailAsync_ReturnsCorrectProductCount()
    {
        var result = await _service.GetCustomerDetailAsync("ALFKI");

        Assert.NotNull(result);
        var order1 = result.Orders.FirstOrDefault(o => o.OrderId == 10248);
        var order2 = result.Orders.FirstOrDefault(o => o.OrderId == 10249);

        Assert.NotNull(order1);
        Assert.Equal(2, order1.ProductCount);

        Assert.NotNull(order2);
        Assert.Equal(1, order2.ProductCount);
    }

    [Fact]
    public async Task GetCustomerDetailAsync_ReturnsCorrectOrderDates()
    {
        var result = await _service.GetCustomerDetailAsync("ALFKI");

        Assert.NotNull(result);
        var order1 = result.Orders.FirstOrDefault(o => o.OrderId == 10248);

        Assert.NotNull(order1);
        Assert.Equal(new DateTime(2024, 1, 1), order1.OrderDate);
    }

    [Fact]
    public async Task GetCustomerDetailAsync_WithNoOrders_ReturnsEmptyOrderList()
    {
        var result = await _service.GetCustomerDetailAsync("ANTON");

        Assert.NotNull(result);
        Assert.Empty(result.Orders);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
