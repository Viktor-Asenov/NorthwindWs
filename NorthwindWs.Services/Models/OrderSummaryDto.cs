namespace NorthwindWs.Services.Models;

public class OrderSummaryDto
{
    public int OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal TotalValue { get; set; }
    public int ProductCount { get; set; }
}
