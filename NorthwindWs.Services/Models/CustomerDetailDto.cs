namespace NorthwindWs.Services.Models;

public class CustomerDetailDto
{
    public string CustomerId { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public string? ContactName { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public List<OrderSummaryDto> Orders { get; set; } = new();
}
