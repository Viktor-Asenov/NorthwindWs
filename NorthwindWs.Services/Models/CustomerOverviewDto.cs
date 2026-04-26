namespace NorthwindWs.Services.Models;

public class CustomerOverviewDto
{
    public string CustomerId { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public int TotalOrders { get; set; }
}
