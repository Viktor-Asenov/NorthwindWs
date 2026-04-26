using NorthwindWs.Services.Models;

namespace NorthwindWs.Services.Interfaces;

public interface ICustomerService
{
    Task<List<CustomerOverviewDto>> GetCustomersOverviewAsync();
    Task<CustomerDetailDto?> GetCustomerDetailAsync(string customerId);
}
