using Microsoft.AspNetCore.Mvc;
using NorthwindWs.Services.Interfaces;

namespace NorthwindWs.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomersOverview()
    {
        var customers = await _customerService.GetCustomersOverviewAsync();
        return Ok(customers);
    }

    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetCustomerDetail(string customerId)
    {
        var customer = await _customerService.GetCustomerDetailAsync(customerId);

        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }
}
